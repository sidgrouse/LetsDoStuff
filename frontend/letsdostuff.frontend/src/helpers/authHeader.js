import { useAppContext } from "./libs/contextLib";

export function authHeader() {
    const { token } = useAppContext();

    if (token) {
        console.log('token!!!!!!');
        return { Authorization: `Bearer ${token}` };
    } else {
        console.log('no token-----');
        return {};
    }
}