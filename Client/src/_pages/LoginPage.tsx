import { Redirect } from 'react-router-dom';
import * as React from 'react';
import { Link } from 'react-router-dom';
import { history } from '../_helpers/history';
import { connect } from 'react-redux';
import { Form, Icon, Input, Button, Checkbox, Row, Col, message, Card,Avatar } from 'antd';
const FormItem = Form.Item;
import { UserActions } from '../_actions/UserActions';

class LoginPage extends React.Component<any, any> {
    userActions: UserActions;
    constructor(props: any) {
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

    handleChange(e: any) {
        const { name, value } = e.target;
        this.setState({ [name]: value });
    }

    handleSubmit(e: any) {
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
        return (
            <Row justify="center" align="middle" type="flex" className="login-form-container">
              <Card bordered={false} style={{ width: 450 }}>
                <Form onSubmit={this.handleSubmit} className="login-form">
                  <FormItem style={{ textAlign:'center' }}>
                    <div style={{ textAlign:'center', fontFamily:'Tahoma', fontSize:'22pt', fontWeight:'bold' }} >
                            Product Catalog
                    </div>
                  </FormItem>
                  <FormItem>
                      <Input prefix={<Icon type="user" style={{ color: 'rgba(0,0,0,.25)' }} />} onChange={this.handleChange} placeholder="Username" value={username} name="username" /> 
                  </FormItem>
                  <FormItem>
                      <Input prefix={<Icon type="lock" style={{ color: 'rgba(0,0,0,.25)' }} />} onChange={this.handleChange} type="password" placeholder="Password" value={password} name="password" />
                  </FormItem>
                  <FormItem>
                    <Button type="primary" htmlType="submit" className="login-form-button">
                      Log in
                    </Button>
                  </FormItem>
                </Form>
              </Card>
          </Row>
          );
    }
}

function mapStateToProps(state: any) {
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



