import { jwtDecode } from "jwt-decode";

const useParseToken = (token) => {
    try {
        const jwtInfo = jwtDecode(token);
        const login = jwtInfo.name;
        const role = jwtInfo.Role;
        const id = jwtInfo.sub;

        return {id, login, role};

    }
    catch (error) {
        console.log(error);
        return {}
    }
}

export default useParseToken;