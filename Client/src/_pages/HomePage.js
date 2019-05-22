import * as React from 'react';
import { connect } from 'react-redux';
class HomePage extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        const { user, users } = this.props;
        return (React.createElement("div", null,
            React.createElement("h1", null, "Hello, world!"),
            React.createElement("p", null, "Welcome to your new Product Catalog Application, built with:"),
            React.createElement("ul", null,
                React.createElement("li", null,
                    React.createElement("a", { href: 'https://get.asp.net/' }, "ASP.NET Core"),
                    " and ",
                    React.createElement("a", { href: 'https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx' }, "C#"),
                    " for cross-platform server-side code"),
                React.createElement("li", null,
                    React.createElement("a", { href: 'https://facebook.github.io/react/' }, "React"),
                    " and ",
                    React.createElement("a", { href: 'http://www.typescriptlang.org/' }, "TypeScript"),
                    " for client-side code"),
                React.createElement("li", null,
                    React.createElement("a", { href: 'https://webpack.github.io/' }, "Webpack"),
                    " for building and bundling client-side resources"),
                React.createElement("li", null,
                    React.createElement("a", { href: 'https://ant.design/docs/react/introduce' }, "Ant Design"),
                    " for layout and styling"))));
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
const connectedHomePage = connect(mapStateToProps)(HomePage);
export { connectedHomePage as HomePage };
//# sourceMappingURL=HomePage.js.map