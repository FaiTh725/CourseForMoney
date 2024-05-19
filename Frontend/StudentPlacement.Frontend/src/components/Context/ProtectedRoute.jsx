import { Navigate, Outlet, useLocation, useNavigate } from "react-router-dom";
import useAuth from "../../hooks/useAuth";
import { useContext, useEffect, useState } from "react";
import Load from "../Load/Load";
import useParseToken from "../../hooks/useParseToken";
import AuthContext from "./AuthProvider";
const ProtectedRoute = () => {
    const [load, setLoad] = useState(true);
    const { auth, setAuth } = useContext(AuthContext);
    const [login, setLogin] = useState("");
    const location = useLocation();
    const navigate = useNavigate();

    useEffect(() => {
        var token = localStorage.getItem("token");
        if (!token) {
            navigate("/Auth");
        }

        const { id, login, role } = useParseToken(token);

        setAuth({ id, login, role });
        setLoad(false);
    }, []);

    if (load == true) {
        return (
            <Load />
        )
    }

    return (
        auth?.login ? <Outlet /> : <Navigate state={{ from: location }} to="/Auth" />
    )
}

export default ProtectedRoute;