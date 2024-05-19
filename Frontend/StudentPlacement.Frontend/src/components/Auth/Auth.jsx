import { useNavigate } from "react-router-dom";
import api from "../../api/helpAxios";
import styles from "./Auth.module.css"
import { useEffect, useRef, useState } from "react";

const Auth = () => {
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const errorMessage = useRef(null);
    const navigate = useNavigate();

    const Enter = async (e) => {
        e.preventDefault();

        try {
            const response = await api.post("/Account/Enter", {
                login: login,
                password: password
            }, {
                withCredentials: true
            });

            if (response.data.statusCode == 2) {
                errorMessage.current.textContent = response.data.description;
                return;
            }

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
                return;
            }

            localStorage.setItem("token", response.data.data.token);
            
            navigate("/Home");
        }
        catch (error) {
            console.log(error);
        }
    }

    useEffect(() => {
        errorMessage.current.textContent = "";
    }, [login, password]);

    return (
        <main className={styles.main}>
            <div className={styles.inner}>
                <div className={styles.head}>
                    <img src="" alt="logo" />
                </div>
                <form onSubmit={(e) => { Enter(e) }} className={styles.form}>
                    <div className={styles.inputData}>
                        <label htmlFor="">Логин</label>
                        <input type="text" onChange={(e) => { setLogin(e.target.value) }} placeholder="логин" />
                    </div>
                    <div className={styles.inputData}>
                        <label htmlFor="">Пароль</label>
                        <input type="text" onChange={(e) => { setPassword(e.target.value) }} placeholder="пароль" />
                    </div>
                    <div>
                        <label ref={errorMessage}></label>
                    </div>
                    <div>
                        <button type="submit">Войти</button>
                    </div>
                </form>
            </div>
        </main>
    )
}

export default Auth;