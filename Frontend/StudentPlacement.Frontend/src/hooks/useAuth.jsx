import { useContext } from "react"
import { AuthProvider } from "../components/Context/AuthProvider";

const useAuth = () => {
    return useContext(AuthProvider);
}

export default useAuth;