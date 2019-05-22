import * as React from "react";
import { connect } from "react-redux";
import { AntGrid } from "../_components/Table/AntGrid";
import { config } from "../_helpers/config";
import { message, Upload, Icon, Popconfirm } from "antd";
import { authHeader } from "../_helpers/auth-header";
class ProductCatalogPage extends React.Component {
    constructor(props) {
        super(props);
        this.onSelectedRow = (selectedRowKeys, selectedRows) => {
            const photoStringFormat = selectedRows[0].photoStringFormat;
            const productCode = selectedRows[0].code;
            const selectedRow = selectedRows[0];
            this.setState({ photoStringFormat: photoStringFormat, productCode: productCode, selectedRow: selectedRow, image: undefined, });
        };
        this.beforeUpload = (file, fileList) => {
            const isJPG = file.type === "image/jpeg";
            if (!isJPG) {
                message.error("You can only upload JPG file!");
            }
            const isLt2M = file.size / 1024 / 1024 < 5;
            if (!isLt2M) {
                message.error("Image must smaller than 2MB!");
            }
            return isJPG && isLt2M;
        };
        this.handleChange = this.handleChange.bind(this);
        this.saveImage = this.saveImage.bind(this);
    }
    getBase64(img, callback) {
        const reader = new FileReader();
        reader.addEventListener('load', () => callback(reader.result));
        reader.readAsDataURL(img);
    }
    handleChange(info) {
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
        this.getBase64(info.file.originFileObj, (image) => {
            this.setState({ image: image });
        });
    }
    saveImage(imageFile) {
        const productCode = this.state && this.state.productCode ? this.state.productCode : undefined;
        const selectedRow = this.state && this.state.selectedRow ? this.state.selectedRow : undefined;
        if (productCode === undefined) {
            message.error("Choose Product!");
            return;
        }
        const requestData = {
            productCode: productCode,
            photoStringFormat: imageFile.substring(23, imageFile.length)
        };
        const requestOptions = {
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
    cancelImageSave(e) {
        console.log(e);
        message.error("Saving is canceled");
    }
    handleResponse(response) {
        return new Promise((resolve, reject) => {
            if (response.ok) {
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
                message.error(response.text());
            }
        });
    }
    handleError(error) {
        message.error(error.message);
        return Promise.reject(error && error.message);
    }
    render() {
        const productColumns = [
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
        let image = undefined;
        if (this.state && this.state.image) {
            image = this.state.image;
        }
        else if (this.state && this.state.photoStringFormat) {
            image = "data:image/png;base64," + this.state.photoStringFormat;
        }
        return (React.createElement("div", null,
            React.createElement(AntGrid, { searchActionUrl: config.apiUrl + "api/product/SearchProductsAsync", saveActionUrl: config.apiUrl + "api/product/SaveProductAsync", deleteActionUrl: config.apiUrl + "api/product/RemoveProductAsync", operationColumnTitle: "Operations", columns: productColumns, title: "Products", onSelectedRow: this.onSelectedRow, rowKey: "id" }),
            React.createElement(Upload, Object.assign({}, tProps, { showUploadList: false, onChange: this.handleChange, listType: "picture-card", multiple: false, beforeUpload: this.beforeUpload }),
                image && React.createElement("img", { width: 400, height: 250, src: image }),
                React.createElement(Icon, { type: "upload" }),
                " Upload"),
            React.createElement(Popconfirm, { style: { marginBottom: 16, textAlign: "left", alignContent: "Button" }, title: "Are you sure save this image?", onConfirm: (event) => this.saveImage(image), onCancel: this.cancelImageSave, okText: "Yes", cancelText: "No" },
                React.createElement("a", { href: "#" }, "Save"))));
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
const connectedProductCatalogPage = connect(mapStateToProps)(ProductCatalogPage);
export { connectedProductCatalogPage as ProductCatalogPage };
//# sourceMappingURL=ProductCatalog.js.map