import * as React from 'react';
import { connect } from 'react-redux';
import { Icon } from 'antd';
import { Link, NavLink } from 'react-router-dom';
class HomePage extends React.Component<any, any> {
    constructor(props: any) {
        super(props);
    }

    render() {
        const { user, users } = this.props;
        return (<div>
                    <h1>Hello, world!</h1>
                    <p>Welcome to your new Product Catalog Application, built with:</p>
                    <ul>
                        <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
                        <li><a href='https://facebook.github.io/react/'>React</a> and <a href='http://www.typescriptlang.org/'>TypeScript</a> for client-side code</li>
                        <li><a href='https://webpack.github.io/'>Webpack</a> for building and bundling client-side resources</li>
                        <li><a href='https://ant.design/docs/react/introduce'>Ant Design</a> for layout and styling</li>
                    </ul>
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

const connectedHomePage = connect(mapStateToProps)(HomePage);
export { connectedHomePage as HomePage };