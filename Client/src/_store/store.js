import { Store } from "react-stores";
export var CommonStore;
(function (CommonStore) {
    const initialState = {
        counter: 0
    };
    CommonStore.store = new Store(initialState);
})(CommonStore || (CommonStore = {}));
//# sourceMappingURL=store.js.map