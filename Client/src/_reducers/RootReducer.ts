import { combineReducers } from 'redux';

import { authentication } from './AuthenticationReducer';
// import { registration } from './registration.reducer';
import { users } from './UsersReducer';
import { alert } from './AlertReducer';

const rootReducer = combineReducers({
  authentication,
  // registration,
  users,
  alert
});

export default rootReducer;