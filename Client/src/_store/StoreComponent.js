import * as React from "react";
import { StoreComponent } from "react-stores";
import { CommonStore } from "./store";
export class AppTest extends StoreComponent {
    constructor() {
        super({
            common: CommonStore.store
        });
    }
    increaseCommon() {
        // You can mutate stores as local component state values
        this.stores.common.setState({
            counter: this.stores.common.state.counter + 1
        });
    }
    increaseLocal() {
        // Also you can use local state as natural React.Component
        this.setState({
            counter: this.state.counter + 1
        });
    }
    render() {
        return (React.createElement("div", null,
            React.createElement("p", null,
                "Component name: ",
                this.props.name),
            React.createElement("p", null,
                "Common counter value: ",
                this.stores.common.state.counter.toString()),
            React.createElement("p", null,
                "Local counter value: ",
                this.state.counter.toString()),
            React.createElement("button", { onClick: this.increaseCommon.bind(this) }, "Increase common counter value"),
            React.createElement("button", { onClick: this.increaseLocal.bind(this) }, "Increase local counter value")));
    }
}
//# sourceMappingURL=StoreComponent.js.map