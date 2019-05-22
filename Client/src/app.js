import * as React from "react";
import { Router, Route, Switch as RouterSwitch, Redirect } from "react-router-dom";
import { connect } from "react-redux";
import { Layout, Alert, Icon, Menu, Dropdown, Button, message, Switch, Tabs, Row, Col, Input } from "antd";
import { history } from "./_helpers/history";
import { AlertActions } from "./_actions/AlertActions";
import { HomePage } from "./_pages/HomePage";
import { LoginPage } from "./_pages/LoginPage";
import { ProductCatalogPage } from "./_pages/ProductCatalog";
import { NavLink } from "react-router-dom";
const Search = Input.Search;
const { Header, Content, Sider } = Layout;
const TabPane = Tabs.TabPane;
import { UserActions } from "./_actions/UserActions";
class App extends React.Component {
    constructor(props) {
        super(props);
        this.changeMode = (value) => {
            this.setState({
                mode: value ? "inline" : "vertical"
            });
        };
        this.changeTheme = (value) => {
            this.setState({
                theme: value ? "light" : "dark",
                background: value ? "#ffffff" : "#2C3C45",
                fontColor: value ? "#666666" : "#ffffff",
            });
        };
        this.CustomRouter = () => {
            if (this.state.pathname != location.pathname) {
                this.setState({ pathname: location.pathname });
                this.RouterChange(location.pathname.replace("/", ""));
            }
        };
        this.onChange = (activeKey) => {
            this.setState({ activeKey: activeKey });
            history.push(activeKey.replace("tabPane", ""));
        };
        this.add = (code, content, icon, title) => {
            const panes = this.state.panes;
            const activeKey = `tabPane${code}`;
            panes.push({ title: React.createElement("span", null,
                    React.createElement(Icon, { type: icon }),
                    title), content: content, key: activeKey });
            this.setState({ panes: panes, activeKey: activeKey });
        };
        this.select = (code) => {
            const panes = this.state.panes;
            const activeKey = `tabPane${code}`;
            this.setState({ panes: panes, activeKey: activeKey });
        };
        this.remove = (code) => {
            const panes = this.state.panes.filter((pane) => pane.key !== code);
            this.setState({ panes: panes });
            if (panes.length > 0) {
                this.setState({ activeKey: panes[panes.length - 1].key });
                history.push(panes[panes.length - 1].key.replace("tabPane", ""));
            }
            else {
                history.push("/");
            }
        };
        this.onEdit = (code, action) => {
            this.remove(code);
        };
        this.clickLogOut = () => {
            this.setState({ iconLoading: true });
            this.props.dispatch(new UserActions().logout());
            history.push("/home");
        };
        this.toggle = () => {
            this.setState({
                collapsed: !this.state.collapsed,
            });
        };
        this.state = {
            panes: [],
            theme: "dark",
            background: "#2C3C45",
            fontColor: "#ffffff"
        };
        const { dispatch } = this.props;
        history.listen((location, action) => {
            //clear alert on location change
            dispatch(AlertActions.clear());
        });
    }
    handleButtonClick(e) {
        message.info("Click on left button.");
        console.log("click left button", e);
    }
    handleMenuClick(e) {
        message.info("Click on menu item.");
        console.log("click", e);
    }
    RouterChange(code) {
        if (code == "")
            code = "home";
        var ifExist = false;
        this.state.panes.forEach((pane, i) => {
            if (pane.key === `tabPane${code}`) {
                ifExist = true;
                this.select(code);
            }
        });
        if (!ifExist && this.state.activeKey != `tabPane${code}`) {
            ifExist = true;
            if (code == "home")
                this.add("home", React.createElement(HomePage, null), "home", "Home Page");
            // else if(code == "counter")  this.add(code, <CounterPage/>, "video-camera", "CounterPage");
            else if (code == "productcatalog")
                this.add(code, React.createElement(ProductCatalogPage, null), "upload", "Product Catalog");
            // else if(code == "cards") this.add(code, <CardsPage/>, "book", "Cards Page");
            //else if(code == "applicationdefinition")this.add(code, <ApplicationDefinition/>, "appstore", "Application Definition");
            else {
                ifExist = false;
            }
        }
        if (this.state.showLayout != ifExist) {
            this.setState({ showLayout: ifExist });
        }
        return React.createElement("span", null);
    }
    componentWillUpdate() {
        this.CustomRouter();
    }
    render() {
        const { user, users, mode, theme, collapsed } = this.props;
        const logoutMenu = (React.createElement(Menu, { style: { marginTop: 5, background: "#ff5622", marginRight: 5, color: "white", border: "none" } },
            React.createElement(Menu.Item, { key: "setting:1" },
                React.createElement(Button, { type: "primary", icon: "logout", loading: this.state.iconLoading, onClick: this.clickLogOut }, "Logout!"))));
        const { alert } = this.props;
        return (React.createElement(Layout, null,
            alert.message &&
                React.createElement("div", null,
                    React.createElement(Alert, { type: alert.type, message: alert.message, banner: true, closable: true })),
            React.createElement(Router, { history: history },
                React.createElement("div", { style: { height: "100%" } },
                    localStorage.getItem("user") != null ?
                        React.createElement(Layout, null,
                            React.createElement(Sider, { className: !user ? "hidden" : "", style: { background: this.state.background, color: this.state.fontColor }, trigger: null, collapsible: true, collapsed: this.state.collapsed },
                                React.createElement("div", null,
                                    React.createElement(Menu, { defaultSelectedKeys: ["1"], defaultOpenKeys: ["sub1"], mode: this.state.mode, theme: this.state.theme },
                                        React.createElement(Menu.Item, { key: "1" },
                                            React.createElement("span", null,
                                                React.createElement(Icon, { type: "home" }),
                                                React.createElement("span", { className: "menuItemText" }, "Home")),
                                            React.createElement(NavLink, { to: "/", exact: true, activeClassName: "active" })),
                                        React.createElement(Menu.Item, { key: "2" },
                                            React.createElement("span", null,
                                                React.createElement(Icon, { type: "upload" }),
                                                React.createElement("span", { className: "menuItemText" }, "Product Catalog ")),
                                            React.createElement(NavLink, { to: "/productcatalog", activeClassName: "active" })),
                                        React.createElement(Menu.Item, { key: "4" },
                                            React.createElement("span", null,
                                                React.createElement(Icon, { type: "user" }),
                                                React.createElement("span", { className: "menuItemText" }, "Login")),
                                            React.createElement(NavLink, { to: "/login", activeClassName: "active" }))),
                                    React.createElement("hr", { style: { borderStyle: "solid", borderWidth: "1px", borderColor: "lightgray" } }),
                                    React.createElement("div", { style: { marginLeft: 10, marginTop: 10 } },
                                        React.createElement(Switch, { size: "small", onChange: this.changeMode }),
                                        " ",
                                        React.createElement("span", { style: { background: this.state.background, color: this.state.fontColor, fontSize: "9pt" } }, " Change Mode"),
                                        React.createElement("span", { style: { margin: "0 1em" } }),
                                        React.createElement("br", null),
                                        React.createElement(Switch, { size: "small", onChange: this.changeTheme }),
                                        React.createElement("span", { style: { background: this.state.background, color: this.state.fontColor, fontSize: "9pt" } }, " Change Theme"),
                                        React.createElement("br", null)))),
                            React.createElement(Layout, null,
                                React.createElement(Header, { className: !user ? "hidden" : "", style: { background: this.state.background, padding: 0 } },
                                    React.createElement(Row, { gutter: 8, justify: "start" },
                                        React.createElement(Col, { span: 3, style: { maxWidth: "60px", minWidth: "60px" } },
                                            React.createElement(Icon, { className: "trigger", type: this.state.collapsed ? "menu-unfold" : "menu-fold", onClick: this.toggle })),
                                        React.createElement(Col, { span: 9 },
                                            React.createElement(Search, { placeholder: "input search text", onSearch: value => console.log(value), enterButton: true })),
                                        React.createElement(Col, { span: 12 }),
                                        React.createElement(Col, { span: 3, style: { maxWidth: "90px", minWidth: "90px" } },
                                            React.createElement(Dropdown, { overlay: logoutMenu },
                                                React.createElement(Button, { style: { marginTop: 15, float: "right", background: this.state.background, marginRight: 15, color: this.state.fontColor, border: "none" } },
                                                    user ? user.firstName : "",
                                                    React.createElement(Icon, { type: "user" })))))),
                                React.createElement(Content, { style: { margin: "5px 5px", paddingLeft: 10, background: "#fff", minHeight: 280 } },
                                    React.createElement("div", null,
                                        React.createElement(Tabs, { style: { margin: "5px 5px", background: "#fff" }, animated: true, hideAdd: true, onChange: this.onChange, activeKey: this.state.activeKey, type: "editable-card", onEdit: this.onEdit }, this.state.panes.map((pane) => (React.createElement(TabPane, { tab: pane.title, key: pane.key }, pane.content))))))))
                        :
                            React.createElement(RouterSwitch, null,
                                React.createElement(Route, { path: "/login", component: LoginPage }),
                                React.createElement(Redirect, { from: "*", to: "/login" })),
                    localStorage.getItem("user") != null ?
                        React.createElement(RouterSwitch, null,
                            React.createElement(Redirect, { from: "/login", to: "/" }))
                        : React.createElement("div", null)))));
    }
}
function mapStateToProps(state) {
    const { alert, users, authentication, mode, theme, collapsed } = state;
    const { user } = authentication;
    return {
        alert,
        user,
        users,
        mode,
        theme,
        collapsed
    };
}
const connectedApp = connect(mapStateToProps)(App);
export { connectedApp as App };
//# sourceMappingURL=app.js.map