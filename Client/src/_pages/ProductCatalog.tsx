import * as React from "react";
import { connect } from "react-redux";
import { AntGrid, AntGridColumn } from "../_components/Table/AntGrid";
import { config } from "../_helpers/config";
import { message, Upload, Button, Icon, Popconfirm  } from "antd";
import ButtonGroup from "antd/lib/button/button-group";
import { RcFile, UploadListType } from "antd/lib/upload/interface";
import { authHeader } from "../_helpers/auth-header";

class ProductCatalogPage extends React.Component<any, any> {
    constructor(props: any) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.saveImage = this.saveImage.bind(this);
    }
    onSelectedRow = (selectedRowKeys: string[] | number[], selectedRows: any[]) => {
        const photoStringFormat = selectedRows[0].photoStringFormat;
        const productCode = selectedRows[0].code;
        const selectedRow = selectedRows[0];
        this.setState({ photoStringFormat: photoStringFormat, productCode: productCode, selectedRow: selectedRow, image: undefined, });
    }

    beforeUpload = (file: RcFile, fileList: RcFile[]) => {
        const isJPG = file.type === "image/jpeg";
        if (!isJPG) {
          message.error("You can only upload JPG file!");
        }
        const isLt2M = file.size / 1024 / 1024 < 5;
        if (!isLt2M) {
          message.error("Image must smaller than 2MB!");
        }
        return isJPG && isLt2M;
      }

    getBase64(img: any, callback: any) {
        const reader = new FileReader();
        reader.addEventListener('load', () => callback(reader.result));
        reader.readAsDataURL(img);
      }

      handleChange(info: any) {
        let fileList = [...info.fileList];

        // 1. Limit the number of uploaded files
        // Only to show two recent uploaded files, and old ones will be replaced by the new
        fileList = fileList.slice(-1);
        // 2. Read from response and show file link
        fileList = fileList.map((file) => {
          if (file.response) {
            // Component will show file.url as link
            file.url = file.response.url;
          }
          return file;
        });
        this.setState({ fileList });
          this.getBase64(info.file.originFileObj,
            (image: any) => {
              this.setState({ image: image});
            }
          );
    }
    saveImage(imageFile: any) {
        const productCode = this.state && this.state.productCode ? this.state.productCode : undefined;
        const selectedRow = this.state && this.state.selectedRow ? this.state.selectedRow : undefined;
        if (productCode === undefined) {
            message.error("Choose Product!");
            return;
        }
        const requestData: any = {
            productCode: productCode,
            photoStringFormat: imageFile.substring(23, imageFile.length)
        };
        const requestOptions: any = {
            method: "POST",
            headers: authHeader(),
            body: JSON.stringify(requestData),
        };
        fetch(config.apiUrl + "api/product/SaveProductImageAsync", requestOptions)
        .then(this.handleResponse, this.handleError)
        .then(result => {
            message.success("Image Saved!");
            selectedRow.photoStringFormat = requestData.photoStringFormat;
        });
    }
    cancelImageSave(e: any) {
        console.log(e);
        message.error("Saving is canceled");
    }
    handleResponse(response: any) {
        return new Promise((resolve, reject) => {
            if (response.ok) {
                const contentType = response.headers.get("content-type");
                if (contentType && contentType.includes("application/json")) {
                    response.json().then((json: any) => resolve(json));
                } else {
                    resolve();
                }
            } else {
                // return error message from response body
                message.error(response.text());
            }
        });
    }
    handleError(error: any) {
        message.error(error.message);
        return Promise.reject(error && error.message);
    }

    render() {
        const productColumns: AntGridColumn[]  =
        [
            {
                key: "Code",
                title: "Product Code",
                dataIndex: "code",
                editable: true
            },
            {
                key: "Name",
                title: "Product Name",
                dataIndex: "name",
                editable: true
            },
            {
                key: "Price",
                title: "Price",
                dataIndex: "price",
                editable: true,
                inputType: "number",
            },
            {
                key: "LastUpdatedDate",
                title: "Last Updated Date",
                dataIndex: "lastUpdatedDate",
                editable: false,
                inputType: "date"
            }
        ];

        let tProps = {};
        if (this.state && this.state.fileList !== undefined) {
            tProps = { fileList: this.state.fileList };
        }

        let image: any = undefined;
        if (this.state && this.state.image) {
            image = this.state.image;
        } else if (this.state &&  this.state.photoStringFormat) {
            image = "data:image/png;base64," + this.state.photoStringFormat;
        }
        return (
                <div>
                <AntGrid searchActionUrl = {config.apiUrl + "api/product/SearchProductsAsync"}
                        saveActionUrl = {config.apiUrl + "api/product/SaveProductAsync"}
                        deleteActionUrl = {config.apiUrl + "api/product/RemoveProductAsync"}
                        operationColumnTitle = "Operations"
                        columns = { productColumns }
                        title = "Products"
                        onSelectedRow = {this.onSelectedRow}
                        rowKey="id"
                        />
                <Upload {...tProps}
                showUploadList={false}
                onChange={this.handleChange}
                listType="picture-card"
                    multiple={false}
                    beforeUpload={this.beforeUpload}>
                 {image && <img width={400} height={250} src={image} />}
                    <Icon type="upload" /> Upload
                </Upload>
                <Popconfirm
                style={{ marginBottom: 16, textAlign: "left", alignContent: "Button"}}
                title="Are you sure save this image?" onConfirm={(event: any) => this.saveImage(image)}
                onCancel={this.cancelImageSave} okText="Yes" cancelText="No">
                        <a href="#">Save</a>
                </Popconfirm>
            </div>
        );
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

const connectedProductCatalogPage = connect(mapStateToProps)(ProductCatalogPage);
export { connectedProductCatalogPage as ProductCatalogPage };