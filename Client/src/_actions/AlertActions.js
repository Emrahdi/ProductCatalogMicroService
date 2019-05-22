import { alertConstants } from '../_constants/AlertConstants';
export const AlertActions = {
    success(message) {
        return { type: alertConstants.SUCCESS, message };
    },
    error(message) {
        return { type: alertConstants.ERROR, message };
    },
    clear() {
        return { type: alertConstants.CLEAR };
    }
};
//# sourceMappingURL=AlertActions.js.map