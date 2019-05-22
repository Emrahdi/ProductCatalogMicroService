import * as React from 'react';
import { connect } from 'react-redux';
import { Form, Icon, Input, Button, Row, Card } from 'antd';
const FormItem = Form.Item;
import { UserActions } from '../_actions/UserActions';
class LoginPage extends React.Component {
    constructor(props) {
        super(props);
        this.userActions = new UserActions();
        // reset login status
        //this.props.dispatch(this.userActions.logout());
        this.state = {
            username: '',
            password: '',
            submitted: false
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleChange(e) {
        const { name, value } = e.target;
        this.setState({ [name]: value });
    }
    handleSubmit(e) {
        e.preventDefault();
        this.setState({ submitted: true });
        const { username, password } = this.state;
        const { dispatch } = this.props;
        if (username && password) {
            dispatch(this.userActions.login(username, password));
        }
    }
    render() {
        const { loggingIn } = this.props;
        const { username, password, submitted } = this.state;
        const { user } = this.props;
        return (React.createElement(Row, { justify: "center", align: "middle", type: "flex", className: "login-form-container" },
            React.createElement(Card, { bordered: false, style: { width: 450 } },
                React.createElement(Form, { onSubmit: this.handleSubmit, className: "login-form" },
                    React.createElement(FormItem, { style: { textAlign: 'center' } },
                        React.createElement("div", { style: { textAlign: 'center', fontFamily: 'Tahoma', fontSize: '22pt', fontWeight: 'bold' } }, "Product Catalog")),
                    React.createElement(FormItem, null,
                        React.createElement(Input, { prefix: React.createElement(Icon, { type: "user", style: { color: 'rgba(0,0,0,.25)' } }), onChange: this.handleChange, placeholder: "Username", value: username, name: "username" })),
                    React.createElement(FormItem, null,
                        React.createElement(Input, { prefix: React.createElement(Icon, { type: "lock", style: { color: 'rgba(0,0,0,.25)' } }), onChange: this.handleChange, type: "password", placeholder: "Password", value: password, name: "password" })),
                    React.createElement(FormItem, null,
                        React.createElement(Button, { type: "primary", htmlType: "submit", className: "login-form-button" }, "Log in"))))));
    }
}
function mapStateToProps(state) {
    const { users, authentication } = state;
    const { user, loggingIn } = state.authentication;
    return {
        loggingIn,
        user,
        users,
        authentication
    };
}
const connectedLoginPage = connect(mapStateToProps)(LoginPage);
export { connectedLoginPage as LoginPage };
//# sourceMappingURL=LoginPage.js.map