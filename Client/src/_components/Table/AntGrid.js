var __rest = (this && this.__rest) || function (s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) if (e.indexOf(p[i]) < 0)
            t[p[i]] = s[p[i]];
    return t;
};
import * as React from "react";
import { connect } from "react-redux";
import { Collapse, notification, Button, Icon, Row, Card, Table, Input, InputNumber, Popconfirm, Form, DatePicker, Col } from "antd";
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
};
const exportToCsv = new ExportToCsv(options);
const successMessage = (description) => {
    notification["success"]({
        message: "Success!",
        description: description
    });
};
const errorMessage = (description) => {
    notification["error"]({
        message: "Error!",
        description: description
    });
};
const warningMessage = (description) => {
    notification["warning"]({
        message: "Warning!",
        description: description
    });
};
const infoMessage = (description) => {
    notification["info"]({
        message: "Info:",
        description: description
    });
};
const EditableFormRow = Form.create({})((_a) => {
    var props = __rest(_a, []);
    return (React.createElement(EditableContext.Provider, { value: props.form },
        React.createElement("tr", Object.assign({}, props))));
});
const dateFormat = "DD.MM.YYYY";
class AntEditableCell extends React.Component {
    constructor(props) {
        super(props);
        this.getInput = (record) => {
            if (this.props.inputtype === "number") {
                return React.createElement(InputNumber, { id: this.props.dataindex, onChange: this.onNumberChange, defaultValue: record[2] });
            }
            else if (this.props.inputtype === "date__TODO") {
                return React.createElement(DatePicker, { id: this.props.dataindex });
            }
            return React.createElement(Input, { id: this.props.dataindex, onChange: this.onTextChange, defaultValue: record[2] });
        };
        this.onNumberChange = (value) => {
            const { handlesave } = this.props;
            handlesave(this.state.cellName, value);
        };
        this.onTextChange = (e) => {
            const { name, value } = e.target;
            const { handlesave } = this.props;
            handlesave(this.state.cellName, value);
        };
        this.state = { cellName: this.props.dataindex };
        this.onNumberChange = this.onNumberChange.bind(this);
        this.onTextChange = this.onTextChange.bind(this);
    }
    render() {
        const _a = this.props, { editing, dataindex, title, inputtype, record, index } = _a, restProps = __rest(_a, ["editing", "dataindex", "title", "inputtype", "record", "index"]);
        return (React.createElement(EditableContext.Consumer, null, (form) => {
            return (React.createElement("td", { style: { margin: 0, padding: 5, marginBottom: 0 } }, editing ? (React.createElement(FormItem, { style: { margin: 0, marginBottom: 0 } }, this.getInput(restProps.children))) : restProps.children));
        }));
    }
}
class AntGrid extends React.Component {
    constructor(props) {
        super(props);
        this.onCellChange = (activeCellName, value) => {
            const dataTable = [...this.state.dataTable];
            const index = dataTable.findIndex(item => item[this.props.rowKey] === this.state.editingKey);
            const dataRow = dataTable[index];
            const currentDataTable = dataTable[index];
            currentDataTable[activeCellName] = value;
            dataTable.splice(index, 1, Object.assign({}, dataRow, currentDataTable));
            this.setState({
                dataTable: dataTable
            });
        };
        this.Search = (e) => {
            const { Code, Name } = this.state;
            const requestOptions = {
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
        };
        this.isBoolEditing = (record) => {
            return record[this.props.rowKey] === this.state.editingKey;
        };
        this.cancel = () => {
            this.setState({ editingKey: "" });
        };
        this.handleAdd = () => {
            const { dataTable, newRowCount } = this.state;
            const newKey = ((newRowCount + 1) * -1).toString();
            const newData = { key: newKey, id: newKey, RowStatus: "New" };
            this.setState({
                dataTable: [...dataTable, newData],
                newRowCount: newRowCount + 1
            });
            this.setState({ editingKey: newKey });
        };
        this.handleDelete = (rowKey) => {
            const dataTable = [...this.state.dataTable];
            const dataRow = dataTable.filter(item => item[this.props.rowKey] === rowKey)[0];
            const requestOptions = {
                method: "DELETE",
                headers: authHeader(),
                body: JSON.stringify(dataRow)
            };
            fetch(this.props.deleteActionUrl, requestOptions)
                .then(this.handleResponse, this.handleError)
                .then(data => {
                this.setState({ dataTable: dataTable.filter(item => item[this.props.rowKey] !== rowKey) });
                infoMessage("Deleted.");
            });
        };
        let operationMode = "serverSidePerRow";
        if (this.props.operationMode) {
            operationMode = this.props.operationMode;
        }
        let operationColumnTitle = "Operations";
        if (this.props.operationColumnTitle) {
            operationColumnTitle = this.props.operationColumnTitle;
        }
        this.state = { dataTable: [], loading: false, selectedRowKeys: [], editingKey: "", newRowCount: 0,
            collapseKey: "1", operationColumnTitle: operationColumnTitle, operationMode: operationMode };
        // this.onSelectChange = this.onSelectChange.bind(this);
        this.renderDataTable = this.renderDataTable.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.onCellChange = this.onCellChange.bind(this);
        this.searchCollapseActive = this.searchCollapseActive.bind(this);
        this.resultCollapseActive = this.resultCollapseActive.bind(this);
        this.exportExcel = this.exportExcel.bind(this);
        this.onSelection = this.onSelection.bind(this);
    }
    // onSelectChange(selectedRowKeys: any) {
    //     this.setState({ selectedRowKeys });
    //     if(this.props.onSelectedRow)this.props.onSelectedRow(selectedRowKeys);
    // }
    onSelection(selectedRowKeys, selectedRows) {
        this.setState({ selectedRowKeys });
        if (this.props && this.props.onSelectedRow) {
            this.props.onSelectedRow(selectedRowKeys, selectedRows);
        }
    }
    renderDataTable(state, dataTable) {
        const columns = [...this.props.columns,
            {
                key: "Operation",
                title: this.state.operationColumnTitle,
                fixed: "right",
                width: 120,
                render: (text, record) => {
                    const editable = this.isBoolEditing(record);
                    return (React.createElement("div", null, editable ? (React.createElement("span", null,
                        React.createElement(EditableContext.Consumer, null, form => (React.createElement("a", { href: "javascript:;", onClick: () => this.save(form, record[this.props.rowKey]), style: { marginRight: 8 } }, "Save"))),
                        React.createElement("span", null, "/ "),
                        React.createElement(Popconfirm, { title: "Sure to cancel?", onConfirm: () => this.cancel() },
                            React.createElement("a", { onClick: () => this.cancel() }, "Cancel")))) : (React.createElement("div", null,
                        React.createElement("a", { onClick: () => this.edit(record[this.props.rowKey]) }, "Edit"),
                        React.createElement("span", null, " / "),
                        React.createElement(Popconfirm, { title: "Sure to delete?", onConfirm: () => this.handleDelete(record[this.props.rowKey]) },
                            React.createElement("a", { href: "javascript:;" }, "Delete"))))));
                }
            }
        ];
        const { selectedRowKeys } = state;
        const rowSelection = {
            selectedRowKeys,
            hideDefaultSelections: true,
            onChange: this.onSelection,
            type: 'radio',
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
        const clmns = columns.map((col) => {
            if (!col.editable) {
                return col;
            }
            return Object.assign({}, col, { onCell: (record) => ({
                    record,
                    inputtype: col.inputType,
                    dataindex: col.dataIndex,
                    key: record[this.props.rowKey],
                    title: col.title,
                    editing: this.isBoolEditing(record),
                    handlesave: this.onCellChange,
                }) });
        });
        const headerNode = React.createElement(Row, null,
            React.createElement(Col, { span: 12 },
                React.createElement("div", { style: { textAlign: "left", width: "100%", height: 31, padding: 0, margin: 0 } },
                    React.createElement("h2", null, this.props.title))),
            React.createElement(Col, { span: 12 },
                React.createElement("div", { style: { textAlign: "right", width: "100%", height: 31, padding: 0, margin: 0 } },
                    React.createElement(ButtonGroup, null,
                        React.createElement(Button, { onClick: this.handleAdd, type: "primary", style: { marginBottom: 16 } }, "Add"),
                        React.createElement(Button, { onClick: this.searchCollapseActive, style: { marginBottom: 16 } }, "Filter"),
                        React.createElement(Button, { onClick: this.exportExcel, style: { marginBottom: 16 } }, "Export Csv")))));
        return (React.createElement(Table, { title: () => (headerNode), size: "small", bordered: true, rowSelection: rowSelection, loading: state.loading, columns: clmns, dataSource: dataTable, rowClassName: (record, index) => "editable-row", components: components, rowKey: this.props.rowKey }));
    }
    edit(rowKey) {
        this.setState({ editingKey: rowKey });
    }
    save(form, rowKey) {
        const dataTable = [...this.state.dataTable];
        const editingKey = this.state.editingKey;
        const selectedRow = dataTable.filter(item => item[this.props.rowKey] == editingKey)[0];
        const requestOptions = {
            method: "POST",
            headers: authHeader(),
            body: JSON.stringify(selectedRow)
        };
        fetch(this.props.saveActionUrl, requestOptions)
            .then(this.handleResponse, this.handleError)
            .then(currentDataTable => {
            const dataTable = [...this.state.dataTable];
            const index = dataTable.findIndex(item => item[this.props.rowKey] === editingKey);
            const dataRow = dataTable[index];
            dataRow.RowStatus = "";
            dataTable.splice(index, 1, Object.assign({}, dataRow, currentDataTable));
            this.setState({
                dataTable: dataTable, editingKey: ""
            });
            successMessage("Saved");
        });
    }
    handleChange(e) {
        const { name, value } = e.target;
        this.setState({ [name]: value });
    }
    searchCollapseActive() {
        this.setState({ collapseKey: "1" });
    }
    exportExcel() {
        exportToCsv.generateCsv(this.state.dataTable);
    }
    resultCollapseActive() {
        this.setState({ collapseKey: "2" });
    }
    render() {
        const labelCol = {
            xs: { span: 24 },
            sm: { span: 8 },
        };
        const wrapperCol = {
            xs: { span: 24 },
            sm: { span: 16 },
        };
        const { Code, Name, Price } = this.state;
        const contents = (this.state.loading ? React.createElement("p", null,
            React.createElement("em", null, "Loading...")) : this.renderDataTable(this.state, this.state.dataTable));
        const content = (React.createElement("div", null, contents));
        return (React.createElement("div", { style: { textAlign: "left" } },
            React.createElement(Collapse, { style: { borderRadius: 25 }, accordion: true, defaultActiveKey: ["1"], activeKey: this.state.collapseKey },
                React.createElement(Panel, { style: { borderRadius: 25 }, showArrow: false, header: React.createElement("div", { style: { height: 20 }, onClick: this.searchCollapseActive },
                        React.createElement("h4", null, "Search")), key: "1" },
                    React.createElement("div", { style: { margin: 0, padding: 0, background: "#f8f8f8", borderColor: "#f8f8f8", borderWidth: "solid", borderRadius: 6 } },
                        React.createElement(Card, { bordered: false, style: { width: 500, margin: 0, padding: 0, background: "#f8f8f8", borderRadius: 6 } },
                            React.createElement(Form, null,
                                React.createElement(FormItem, { style: { marginBottom: 0 }, labelCol: labelCol, wrapperCol: wrapperCol, label: "Product Code" },
                                    React.createElement(Input, { prefix: React.createElement(Icon, { type: "user", style: { color: "rgba(0,0,0,.25)" } }), placeholder: "Enter Product Code", onChange: this.handleChange, defaultValue: Code, name: "Code" })),
                                React.createElement(FormItem, { style: { marginBottom: 0 }, labelCol: labelCol, wrapperCol: wrapperCol, label: "Product Name" },
                                    React.createElement(Input, { prefix: React.createElement(Icon, { type: "lock", style: { color: "rgba(0,0,0,.25)" } }), placeholder: "Enter Product Name", onChange: this.handleChange, defaultValue: Name, name: "Name" })),
                                React.createElement(FormItem, { style: { marginBottom: 0 } },
                                    React.createElement(Row, null,
                                        React.createElement(Col, { xs: 24, sm: 8 }),
                                        React.createElement(Col, { xs: 24, sm: 16 },
                                            React.createElement(ButtonGroup, null,
                                                React.createElement(Button, { type: "primary", htmlType: "submit", onClick: this.Search }, "Search"),
                                                React.createElement(Button, { htmlType: "submit", onClick: this.resultCollapseActive }, "Cancel"))))))))),
                React.createElement(Panel, { style: { borderRadius: 25 }, showArrow: false, header: React.createElement("div", { style: { height: 20 }, onClick: this.resultCollapseActive },
                        React.createElement("h4", null, "Result")), key: "2" }, content))));
    }
    handleResponse(response) {
        return new Promise((resolve, reject) => {
            if (response.ok) {
                // return json if it was returned in the response
                const contentType = response.headers.get("content-type");
                if (contentType && contentType.includes("application/json")) {
                    response.json().then((json) => resolve(json));
                }
                else {
                    resolve();
                }
            }
            else {
                // return error message from response body
                errorMessage("Error:" + response.statusText + "(" + response.status + ")");
                response.statusText().then((statusText) => reject(statusText));
            }
        });
    }
    handleError(error) {
        errorMessage(error.message);
        return Promise.reject(error && error.message);
    }
}
function mapStateToProps(state) {
    const { users, authentication } = state;
    const { user } = authentication;
    return {
        user,
        users
    };
}
const connectedAntGrid = connect(mapStateToProps)(AntGrid);
export { connectedAntGrid as AntGrid };
//# sourceMappingURL=AntGrid.js.map