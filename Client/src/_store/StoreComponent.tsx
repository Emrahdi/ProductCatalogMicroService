import * as React from "react";
import {StoreComponent, Store} from "react-stores";
import {CommonStore} from "./store";
 
interface Props {
    name: string
}
 
interface State {
    counter: number
}
 
interface StoresState {
    common: Store<CommonStore.State>
}
 
export class AppTest extends StoreComponent<Props, State, StoresState> {
    constructor() {
        super({
            common: CommonStore.store
        });
    }
 
    private increaseCommon():void {
        // You can mutate stores as local component state values
        this.stores.common.setState({
            counter: this.stores.common.state.counter + 1
        });
    }
 
    private increaseLocal():void {
        // Also you can use local state as natural React.Component
        this.setState({
            counter: this.state.counter + 1
        });
    }
 
    render() {
        return (
            <div>
                <p>Component name: {this.props.name}</p>
                <p>Common counter value: {this.stores.common.state.counter.toString()}</p>
                <p>Local counter value: {this.state.counter.toString()}</p>
 
                <button onClick={this.increaseCommon.bind(this)}>Increase common counter value</button>
                <button onClick={this.increaseLocal.bind(this)}>Increase local counter value</button>
            </div>
        );
    }
}