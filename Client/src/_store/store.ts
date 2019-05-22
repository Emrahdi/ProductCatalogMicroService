import {Store} from "react-stores";
 
export namespace CommonStore {
    export interface State {
        counter: number
    }
 
    const initialState: State = {
        counter: 0
    };
 
    export let store: Store<State> = new Store<State>(initialState);
}