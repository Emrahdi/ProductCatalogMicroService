import "./css/ant.less";
import * as React from "react";
import { render } from "react-dom";
import { Provider } from "react-redux";
import { store } from "./_helpers/store";
import { App } from "./App";
render(React.createElement(Provider, { store: store },
    React.createElement(App, null)), document.getElementById("app"));
//# sourceMappingURL=index.js.map