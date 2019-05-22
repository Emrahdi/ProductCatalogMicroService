import { config } from "../_helpers/config";
import { authHeader } from "../_helpers/auth-header";

export class UserService {
    login(username: string, password: string) {
        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        };

        return fetch(config.apiUrl + "api/Users/Authenticate", requestOptions)
            .then(this.handleResponse, this.handleError)
            .then((user: any) => {
                // login successful if there's a jwt token in the response
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem("user", JSON.stringify(user));
                }

                return user;
            });
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem("user");
    }

    handleResponse(response: any) {
        return new Promise((resolve, reject) => {
            if (response.ok) {
                // return json if it was returned in the response
                var contentType = response.headers.get("content-type");
                if (contentType && contentType.includes("application/json")) {
                    response.json().then((json: any) => resolve(json));
                } else {
                    resolve();
                }
            } else {
                // return error message from response body
                response.text().then((text: any) => reject(text));
            }
        });
    }

    handleError(error: any) {
        return Promise.reject(error && error.message);
    }
}

// function getAll() {
//     const requestOptions = {
//         method: 'GET',
//         headers: authHeader()
//     };

//     return fetch(config.apiUrl + '/users', requestOptions).then(handleResponse, handleError);
// }

// function getById(id) {
//     const requestOptions = {
//         method: 'GET',
//         headers: authHeader()
//     };

//     return fetch(config.apiUrl + '/users/' + _id, requestOptions).then(handleResponse, handleError);
// }

// function register(user) {
//     const requestOptions = {
//         method: 'POST',
//         headers: { 'Content-Type': 'application/json' },
//         body: JSON.stringify(user)
//     };

//     return fetch(config.apiUrl + '/users/register', requestOptions).then(handleResponse, handleError);
// }

// function update(user) {
//     const requestOptions = {
//         method: 'PUT',
//         headers: { ...authHeader(), 'Content-Type': 'application/json' },
//         body: JSON.stringify(user)
//     };

//     return fetch(config.apiUrl + '/users/' + user.id, requestOptions).then(handleResponse, handleError);
// }

// // prefixed function name with underscore because delete is a reserved word in javascript
// function _delete(id) {
//     const requestOptions = {
//         method: 'DELETE',
//         headers: authHeader()
//     };

//     return fetch(config.apiUrl + '/users/' + id, requestOptions).then(handleResponse, handleError);
// }
