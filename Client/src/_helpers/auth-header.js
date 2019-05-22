export function authHeader() {
    // return authorization header with jwt token
    let user = JSON.parse(localStorage.getItem('user') || 'null');
    if (user && user.token) {
        return { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + user.token };
    }
    else {
        return {};
    }
}
//# sourceMappingURL=auth-header.js.map