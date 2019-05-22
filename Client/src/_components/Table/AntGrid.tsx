import * as React from "react";
import { connect } from "react-redux";
import { Collapse, notification , Button, Icon, Row, Card, Table, Input, InputNumber, Popconfirm, Form, DatePicker, Col  } from "antd";
// import { Link, NavLink } from 'react-router-dom';
import { authHeader } from "../../_helpers/auth-header";
import ButtonGroup from "antd/lib/button/button-group";
import { ExportToCsv } from "export-to-csv";

const FormItem = Form.Item;
const EditableContext = React.createContext({});
const Panel = Collapse.Panel;

const options = {
    fieldSeparator: ";",
    quoteStrings: "\"",
    decimalseparator: ".",
    showLabels: true,
    showTitle: false,
    title: "",
    useBom: true,
    useKeysAsHeaders: true,
    // headers: ['Column 1', 'Column 2', etc...] <-- Won't work with useKeysAsHeaders present!
  };

const exportToCsv = new ExportToCsv(options);

const successMessage = (description: string) => {
  notification["success"]({
    message: "Success!",
    description: description
  });
};

const errorMessage = (description: string) => {
    notification["error"]({
      message: "Error!",
      description: description
    });
  };

const warningMessage = (description: string) => {
notification["warning"]({
    message: "Warning!",
    description: description
});
};

const infoMessage = (description: string) => {
    notification["info"]({
        message: "Info:",
        description: description
    });
};

const EditableFormRow = Form.create({
})(({...props}) => {
  return (
      <EditableContext.Provider value={props.form}>
      <tr {...props} />
    </EditableContext.Provider>
  );
});
const dateFormat: string = "DD.MM.YYYY";

class AntEditableCell extends React.Component<any, any> {
    constructor(props: any) {
        super(props);
        this.state = { cellName: this.props.dataindex};
        this.onNumberChange = this.onNumberChange.bind(this);
        this.onTextChange = this.onTextChange.bind(this);
    }
    getInput = (record: any) => {
      if (this.props.inputtype === "number") {
        return <InputNumber  id={this.props.dataindex} onChange={this.onNumberChange}  defaultValue={record[2]} />;
      }
      else if (this.props.inputtype === "date__TODO") {
        return <DatePicker  id={this.props.dataindex}/>;
      }
      return <Input  id={this.props.dataindex} onChange= {this.onTextChange} defaultValue={record[2]} />;
    }

    onNumberChange = (value: any) => {
        const { handlesave } = this.props;
        handlesave(this.state.cellName, value);
    }

    onTextChange = (e: any)  => {
      const { name, value } = e.target;
      const { handlesave } = this.props;
      handlesave(this.state.cellName,  value);
    }

    render() {
      const {
        editing,
        dataindex,
        title,
        inputtype,
        record,
        index,
        ...restProps
      } = this.props;
      return (
        <EditableContext.Consumer>
          {(form: any) => {
            return (
              <td style={{ margin: 0, padding: 5, marginBottom: 0 }}>
              {editing ? (
                <FormItem style={{ margin: 0, marginBottom: 0 }}>
                  {this.getInput(restProps.children)}
                </FormItem>
              ) : restProps.children}
            </td>
            );
          }}
        </EditableContext.Consumer>
      );
    }
}

export interface AntGridProps {
    searchActionUrl: string;
    saveActionUrl: string;
    deleteActionUrl: string;
    columns: AntGridColumn[];
    title: string;
    operationColumnTitle?: string;
    operationMode?: "clientSide" | "serverSidePerRow";
    onSelectedRow?: (selectedRowKeys: string[] | number[], selectedRows: Object[]) => any;
    rowKey: string;
}


export interface AntGridColumn {
    key: string;
    title: string;
    dataIndex: string;
    editable: boolean;
    inputType?: string;
    visible?: boolean;
}

class AntGrid extends React.Component<AntGridProps, any> {
    constructor(props: any) {
        super(props);

        let operationMode = "serverSidePerRow";
        if (this.props.operationMode) {
            operationMode = this.props.operationMode;
        }

        let operationColumnTitle = "Operations";
        if (this.props.operationColumnTitle) {
            operationColumnTitle = this.props.operationColumnTitle;
        }

        this.state = { dataTable: [], loading: false, selectedRowKeys: [], editingKey: "", newRowCount: 0,
         collapseKey: "1", operationColumnTitle: operationColumnTitle, operationMode: operationMode};
        // this.onSelectChange = this.onSelectChange.bind(this);
        this.renderDataTable = this.renderDataTable.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.onCellChange = this.onCellChange.bind(this);
        this.searchCollapseActive = this.searchCollapseActive.bind(this);
        this.resultCollapseActive = this.resultCollapseActive.bind(this);
        this.exportExcel = this.exportExcel.bind(this);
        this.onSelection = this.onSelection.bind(this);
    }

    onCellChange = (activeCellName: any, value: any) =>  {
        const dataTable = [...this.state.dataTable];
        const index = dataTable.findIndex(item => item[this.props.rowKey] === this.state.editingKey);
        const dataRow = dataTable[index];
        const currentDataTable = dataTable[index];
        currentDataTable[activeCellName] = value;
        dataTable.splice(index, 1, {
            ...dataRow,
            ...currentDataTable,
          });
          this.setState({
            dataTable: dataTable
        });
    }
    // onSelectChange(selectedRowKeys: any) {
    //     this.setState({ selectedRowKeys });
    //     if(this.props.onSelectedRow)this.props.onSelectedRow(selectedRowKeys);
    // }
    onSelection (selectedRowKeys: string[] | number[], selectedRows: Object[]) {
      this.setState({ selectedRowKeys });
      if (this.props && this.props.onSelectedRow) {
        this.props.onSelectedRow(selectedRowKeys, selectedRows);
      }
    }
    Search = (e: any) => {
        const { Code, Name } = this.state;
        const requestOptions: any = {
            method: "GET",
            headers: authHeader(),
        };
      let pCode = Code == undefined ? "" : Code;
      let pName = Name == undefined ? "" : Name;

      const parameters = "?code=" + pCode + "&name=" + pName;
        this.setState({ loading: true });
        fetch(this.props.searchActionUrl + parameters, requestOptions)
        .then(this.handleResponse, this.handleError)
        .then(data => {
            this.setState({ dataTable: data, loading: false, collapseKey: "2" });
        });
    }

    renderDataTable(state: any, dataTable: any) {
        const columns = [...this.props.columns,
            {
                key: "Operation",
                title: this.state.operationColumnTitle,
                fixed: "right",
                width: 120,
                render: (text: any, record: any) => {
                  const editable: boolean = this.isBoolEditing(record);
                  return (
                    <div>
                      {editable ? (
                        <span>
                          <EditableContext.Consumer>
                            {form => (
                              <a
                                href="javascript:;"
                                onClick={() => this.save(form, record[this.props.rowKey])}
                                style={{ marginRight: 8 }}>Save</a>
                            )}
                          </EditableContext.Consumer>
                          <span>/ </span>
                          <Popconfirm
                            title="Sure to cancel?"
                            onConfirm={() => this.cancel()}
                          >
                            <a onClick={() => this.cancel()}>Cancel</a>
                          </Popconfirm>
                        </span>
                      ) : (
                        <div>
                            <a onClick={() => this.edit(record[this.props.rowKey])}>Edit</a><span> / </span>
                            <Popconfirm title="Sure to delete?" onConfirm={() => this.handleDelete(record[this.props.rowKey])}>
                            <a href="javascript:;">Delete</a>
                            </Popconfirm>
                        </div>
                      )}
                    </div>
                  );
                }
            }
        ];

        const { selectedRowKeys } = state;

        const rowSelection = {
          selectedRowKeys,
          hideDefaultSelections: true,
          onChange: this.onSelection,
          type: 'radio' as 'radio',
          columnWidth: 20
            // selectedRowKeys,
            // onChange: this.onSelectChange,
            // hideDefaultSelections: true,
            // onSelection: this.onSelection
        };

        const components = {
            body: {
              row: EditableFormRow,
              cell: AntEditableCell,
            },
          };

          const clmns = columns.map((col: any) => {
            if (!col.editable) {
              return col;
            }
            return {
              ...col,
              onCell: (record: any) => ({
                record,
                inputtype: col.inputType,
                dataindex: col.dataIndex,
                key: record[this.props.rowKey],
                title: col.title,
                editing: this.isBoolEditing(record),
                handlesave: this.onCellChange,
                // columns: this.props.columns.filter(x => x.visible === true),
              }),
            };
          });
          const headerNode =
          <Row>
          <Col span={12}>
          <div style={{ textAlign: "left", width: "100%", height: 31, padding: 0, margin: 0}}  >
          <h2>{this.props.title}</h2>
          </div>
          </Col>
          <Col  span={12}>
          <div style={{ textAlign: "right", width: "100%", height: 31, padding: 0, margin: 0}}  >
          <ButtonGroup>
              <Button onClick={this.handleAdd} type="primary" style={{ marginBottom: 16 }}>
                  Add
              </Button>
              <Button onClick={this.searchCollapseActive} style={{ marginBottom: 16 }}>
                  Filter
              </Button>
              <Button onClick={this.exportExcel} style={{ marginBottom: 16 }}>
                  Export Csv
              </Button>
          </ButtonGroup>
          </div>
          </Col>
          </Row>;

    return (<Table  title={() => (headerNode)}
                    size="small"
                    bordered
                    rowSelection={rowSelection}
                    loading={state.loading}
                    columns={clmns}
                    dataSource={dataTable}
                    rowClassName={(record, index) =>  "editable-row"}
                    components={components}
                    rowKey={this.props.rowKey}
                    />
            );
    }
    isBoolEditing = (record: any): boolean => {
        return record[this.props.rowKey] === this.state.editingKey;
    }
    edit(rowKey: any) {
        this.setState({ editingKey: rowKey });
      }

      save(form: any, rowKey: any) {
        const dataTable = [...this.state.dataTable];
        const editingKey = this.state.editingKey;
        const selectedRow = dataTable.filter(item => item[this.props.rowKey] == editingKey)[0];


        const requestOptions: any = {
            method: "POST",
            headers: authHeader(),
            body: JSON.stringify(selectedRow )
        };

        fetch(this.props.saveActionUrl, requestOptions)
        .then(this.handleResponse, this.handleError)
        .then(currentDataTable => {
            const dataTable = [...this.state.dataTable];
            const index = dataTable.findIndex(item => item[this.props.rowKey] === editingKey);
            const dataRow = dataTable[index];
            dataRow.RowStatus="";
            dataTable.splice(index, 1, {
                ...dataRow,
                ...currentDataTable,
            });
            this.setState({
                dataTable: dataTable, editingKey: ""
            });
            successMessage("Saved");
        });
      }

      cancel = () => {
        this.setState({ editingKey: "" });
      }
      handleChange(e: any) {
        const { name, value } = e.target;
        this.setState({ [name]: value });
    }
    searchCollapseActive() {
        this.setState({  collapseKey: "1" });
    }
    exportExcel() {
        exportToCsv.generateCsv(this.state.dataTable);
    }
    resultCollapseActive() {
        this.setState({  collapseKey: "2" });
    }
    handleAdd = () => {
      const { dataTable, newRowCount } = this.state;
      const newKey = ((newRowCount + 1) * -1).toString();
      const newData = { key: newKey, id: newKey, RowStatus: "New" };
      this.setState({
        dataTable: [...dataTable, newData],
        newRowCount: newRowCount + 1
      });
      this.setState({ editingKey: newKey });
    }

      handleDelete = (rowKey: any) => {
        const dataTable = [...this.state.dataTable];
        const dataRow = dataTable.filter(item => item[this.props.rowKey] === rowKey)[0];

        const requestOptions: any = {
            method: "DELETE",
            headers: authHeader(),
            body: JSON.stringify( dataRow )
        };

        fetch(this.props.deleteActionUrl, requestOptions)
        .then(this.handleResponse, this.handleError)
        .then(data => {
            this.setState({ dataTable: dataTable.filter(item => item[this.props.rowKey] !== rowKey) });
            infoMessage("Deleted.");
        });
      }

    render() {

        const  labelCol = {
              xs: { span: 24 },
              sm: { span: 8 },
        };

        const wrapperCol = {
              xs: { span: 24 },
              sm: { span: 16 },
        };

        const { Code, Name, Price } = this.state;
        const contents = (this.state.loading ? <p><em>Loading...</em></p> : this.renderDataTable(this.state, this.state.dataTable));
        const content = (<div>
            { contents }
        </div>);

        return (
        <div style={{ textAlign: "left"}} >
         <Collapse style={{ borderRadius: 25 }} accordion={true} defaultActiveKey={["1"]} activeKey={this.state.collapseKey} >
            <Panel style={{ borderRadius: 25 }} showArrow={false}   header={<div style={{height: 20}} onClick={this.searchCollapseActive}><h4>Search</h4></div>} key="1">
            <div style={{  margin: 0, padding: 0,  background: "#f8f8f8", borderColor: "#f8f8f8", borderWidth: "solid", borderRadius: 6}}>
            <Card bordered={false} style={{ width: 500, margin: 0, padding: 0, background: "#f8f8f8", borderRadius: 6 }}>
              <Form >
                    <FormItem style={{marginBottom: 0}} labelCol={labelCol} wrapperCol={wrapperCol} label="Product Code">
                        <Input prefix={<Icon type="user" style={{ color: "rgba(0,0,0,.25)" }} />} placeholder="Enter Product Code" onChange={this.handleChange}  defaultValue={Code} name="Code" />
                    </FormItem>
                    <FormItem style={{marginBottom: 0}} labelCol={labelCol} wrapperCol={wrapperCol} label="Product Name">
                        <Input prefix={<Icon type="lock" style={{ color: "rgba(0,0,0,.25)" }} />} placeholder="Enter Product Name" onChange={this.handleChange} defaultValue={Name} name="Name" />
                    </FormItem>
                    <FormItem style={{marginBottom: 0}}>
                        <Row >
                            <Col xs={24} sm={8}/>
                            <Col xs={24} sm={16}>
                            <ButtonGroup>
                                <Button type="primary" htmlType="submit"  onClick={this.Search}>
                                    Search
                                </Button>
                                <Button htmlType="submit"  onClick={this.resultCollapseActive}>
                                    Cancel
                                </Button>
                            </ButtonGroup>
                            </Col>
                        </Row>
                    </FormItem>
                </Form>
            </Card>
            </div>
        </Panel >
            <Panel style={{ borderRadius: 25 }} showArrow={false} header={ <div style={{height: 20}} onClick={this.resultCollapseActive}><h4>Result</h4></div>} key="2">
                { content }
            </Panel>
        </Collapse>
          </div>);
    }
    handleResponse(response: any) {
        return new Promise((resolve, reject) => {
            if (response.ok) {
                // return json if it was returned in the response
                const contentType = response.headers.get("content-type");
                if (contentType && contentType.includes("application/json")) {
                    response.json().then((json: any) => resolve(json));
                } else {
                    resolve();
                }
            } else {
                // return error message from response body
                errorMessage("Error:" + response.statusText + "("+ response.status+")");
                response.statusText().then((statusText: any) => reject(statusText));
            }
        });
    }
    handleError(error: any) {
        errorMessage(error.message);
        return Promise.reject(error && error.message);
    }
}

function mapStateToProps(state: any) {
    const { users, authentication } = state;
    const { user } = authentication;
    return {
        user,
        users
    };
}

const connectedAntGrid = connect(mapStateToProps)(AntGrid);
export { connectedAntGrid as AntGrid };