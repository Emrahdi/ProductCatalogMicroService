import * as React from "react";
import { Router, Route, Switch as RouterSwitch, Redirect } from "react-router-dom";
import { connect } from "react-redux";
import { Layout, Alert, Icon,  Menu, Dropdown, Button, message, Switch, Tabs, Row, Col, Input } from "antd";
import { history } from "./_helpers/history";
import { AlertActions } from "./_actions/AlertActions";
import { HomePage } from "./_pages/HomePage";
import { LoginPage } from "./_pages/LoginPage";
import { ProductCatalogPage } from "./_pages/ProductCatalog";
import { Link, NavLink } from "react-router-dom";
const Search = Input.Search;
const { Header, Content, Sider } = Layout;
const TabPane = Tabs.TabPane;
import { UserActions } from "./_actions/UserActions";

class App extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
        this.state = {
            panes: [],
            theme: "dark"as "dark",
            background: "#2C3C45",
            fontColor: "#ffffff"
        };
        const { dispatch } = this.props;
        history.listen((location, action) => {
            //clear alert on location change
            dispatch(AlertActions.clear());
        });
    }

    changeMode = (value: any) => {
        this.setState({
            mode: value ? "inline" as "inline" : "vertical" as "vertical"
        });
    }

    changeTheme = (value: any) => {
        this.setState({
            theme: value ? "light" as "light" : "dark" as "dark",
            background: value ? "#ffffff" : "#2C3C45",
            fontColor: value ? "#666666" : "#ffffff",
        });
    }

    handleButtonClick(e: any) {
        message.info("Click on left button.");
        console.log("click left button", e);
    }

    handleMenuClick(e: any) {
        message.info("Click on menu item.");
        console.log("click", e);
    }

    CustomRouter = () =>
    {
        if (this.state.pathname != location.pathname)
        {
            this.setState({ pathname: location.pathname });
            this.RouterChange(location.pathname.replace("/", ""));
        }
    }

    RouterChange (code: string) {
        if (code == "")code = "home";
        var ifExist = false;
        this.state.panes.forEach((pane: any, i: any) => {
            if (pane.key === `tabPane${code}`) {
                ifExist = true;
                this.select(code);
            }
        });
        if (!ifExist && this.state.activeKey != `tabPane${code}`)
        {
            ifExist = true;
            if (code == "home")this.add("home", <HomePage/>, "home", "Home Page");
            // else if(code == "counter")  this.add(code, <CounterPage/>, "video-camera", "CounterPage");
            else if (code == "productcatalog")this.add(code, <ProductCatalogPage/>, "upload", "Product Catalog");
            // else if(code == "cards") this.add(code, <CardsPage/>, "book", "Cards Page");
            //else if(code == "applicationdefinition")this.add(code, <ApplicationDefinition/>, "appstore", "Application Definition");
            else
            {
                ifExist = false;
            }
        }
        if (this.state.showLayout != ifExist)
        {
            this.setState({ showLayout: ifExist });
        }
        return <span></span>;
    }

    onChange = (activeKey: string) => {
        this.setState({ activeKey: activeKey });
        history.push(activeKey.replace("tabPane", ""));
    };

    add = (code: string, content: any, icon: string, title: string) => {
        const panes = this.state.panes;
        const activeKey = `tabPane${code}`;
        panes.push({ title: <span><Icon type={icon} />{title}</span>, content: content, key: activeKey });
        this.setState({ panes: panes, activeKey: activeKey });
    };

    select = (code: string) => {
        const panes = this.state.panes;
        const activeKey = `tabPane${code}`;
        this.setState({ panes: panes, activeKey: activeKey });
    };

    remove = (code: any) => {
        const panes = this.state.panes.filter((pane: any) => pane.key !== code);
        this.setState({ panes: panes});
        if (panes.length > 0)
        {
            this.setState({ activeKey: panes[panes.length - 1].key});
            history.push(panes[panes.length - 1].key.replace("tabPane", ""));
        }
        else
        {
            history.push("/");
        }
    };

    onEdit = (code: any, action: any) => {
        this.remove(code);
    }


    componentWillUpdate()
    {
        this.CustomRouter();
    }

    clickLogOut = () => {
        this.setState({ iconLoading: true });
        this.props.dispatch(new UserActions().logout());
        history.push("/home");
      }

    render() {

        const { user, users, mode, theme, collapsed } = this.props;
        const logoutMenu = (<Menu style={{ marginTop: 5 , background: "#ff5622", marginRight: 5, color: "white", border: "none"}} >
                       <Menu.Item key="setting:1">
                       <Button type="primary" icon="logout" loading={this.state.iconLoading} onClick={this.clickLogOut}>
          Logout!
        </Button></Menu.Item>
                        </Menu>);
        const { alert } = this.props;

        return (
        <Layout>
            {alert.message &&
                <div>
                    <Alert type={alert.type} message={alert.message} banner closable />
                </div>
            }
            {/* //https://medium.com/@djoepramono/react-router-4-gotchas-2ecd1282de65 */}
            {/* //Router yerine HashRouter kullanılırsa linkler # ile oluşur */}
            <Router history={history} >
                <div style={{height: "100%"}}>
                    { localStorage.getItem("user") != null ?
                    <Layout>
                        <Sider  className={!user ? "hidden" : ""}
                            style={{ background: this.state.background, color: this.state.fontColor }}
                            trigger={null}
                            collapsible
                            collapsed={this.state.collapsed}
                        >
                            <div>
                                <Menu defaultSelectedKeys={["1"]}
                                defaultOpenKeys={["sub1"]}
                                mode={this.state.mode}
                                theme={this.state.theme}
                                >
                                <Menu.Item key="1">
                                    <span><Icon type="home" /><span className="menuItemText">Home</span></span>
                                    <NavLink to={ "/" } exact activeClassName="active"></NavLink>
                                </Menu.Item>
                                <Menu.Item key="2">
                                    <span><Icon type="upload" /><span className="menuItemText">Product Catalog </span></span>
                                    <NavLink to={ "/productcatalog" } activeClassName="active"></NavLink>
                                </Menu.Item>
                                {/* <Menu.Item key="3">
                                    <span><Icon type="code-o" /><span className="menuItemText">Grid Test Page</span></span>
                                    <NavLink to={ '/gridTestPage' } activeClassName='active'></NavLink>
                                </Menu.Item> */}
                                <Menu.Item key="4">
                                    <span><Icon type="user" /><span className="menuItemText">Login</span></span>
                                    <NavLink to={ "/login" } activeClassName="active"></NavLink>
                                </Menu.Item>
                                </Menu>
                                    <hr style={{borderStyle: "solid", borderWidth: "1px", borderColor: "lightgray"}} />
                                <div style={{marginLeft: 10, marginTop: 10 }}>
                                    <Switch size="small" onChange={this.changeMode} /> <span  style={{background: this.state.background, color: this.state.fontColor, fontSize: "9pt"}}> Change Mode</span>
                                    <span style={{ margin: "0 1em"}}  />
                                    <br />
                                    <Switch size="small" onChange={this.changeTheme} /><span style={{background: this.state.background, color: this.state.fontColor, fontSize: "9pt"}}> Change Theme</span>
                                    <br />
                                </div>
                            </div>
                        </Sider>
                        <Layout>
                            <Header  className={!user ? "hidden" : ""} style={{ background: this.state.background, padding: 0 }}>
                            <Row gutter={8} justify="start" >
                            <Col span={3} style={{maxWidth: "60px", minWidth: "60px"}}>
                            <Icon
                                    className="trigger"
                                    type={this.state.collapsed ? "menu-unfold" : "menu-fold"}
                                    onClick={this.toggle}
                                />
                            </Col>
                            <Col span={9} >
                            <Search
                                placeholder="input search text"
                                onSearch={value => console.log(value)}
                                enterButton
                                />
                            </Col>
                            <Col span={12} />
                            <Col span={3}  style={{maxWidth: "90px", minWidth: "90px"}}>
                            <Dropdown overlay={logoutMenu}>
                                    <Button style={{ marginTop: 15 , float: "right", background: this.state.background, marginRight: 15, color: this.state.fontColor, border: "none"}}>
                                        {user ? user.firstName : ""}<Icon type="user" />
                                    </Button>
                                </Dropdown>
                            </Col>
                            </Row>

                            </Header>
                            <Content style={{ margin: "5px 5px", paddingLeft: 10, background: "#fff", minHeight: 280 }}>
                                <div>
                                    <Tabs
                                    style={{ margin: "5px 5px", background: "#fff" }}
                                    animated
                                    hideAdd
                                    onChange={this.onChange}
                                    activeKey={this.state.activeKey}
                                    type="editable-card"
                                    onEdit={this.onEdit}
                                    >
                                    {this.state.panes.map((pane: any) => (
                                        <TabPane tab={pane.title} key={pane.key} >
                                        {pane.content}
                                        </TabPane>
                                    ))}
                                    </Tabs>
                                </div>
                            </Content>
                        </Layout>
                    </Layout>
                    :
                    <RouterSwitch>
                            <Route path="/login" component ={LoginPage}/>
                            <Redirect from="*" to="/login"/>
                    </RouterSwitch> }

                    { localStorage.getItem("user") != null ?
                    <RouterSwitch>
                        <Redirect from="/login" to="/"/>
                    </RouterSwitch>
                    : <div></div>
                    }
                </div>
            </Router>
        </Layout>);
    }
    toggle = () => {
        this.setState({
            collapsed: !this.state.collapsed,
        });
    }
}

function mapStateToProps(state: any) {
    const { alert, users, authentication,  mode, theme, collapsed } = state;
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
