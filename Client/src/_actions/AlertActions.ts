import { alertConstants } from '../_constants/AlertConstants';

export const AlertActions = {
    success(message: string) {
        return { type: alertConstants.SUCCESS, message };
    },
    error(message: string) {
        return { type: alertConstants.ERROR, message };
    },
    clear() {
        return { type: alertConstants.CLEAR };
    }
};