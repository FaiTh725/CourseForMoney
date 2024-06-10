import styles from "./Home.module.css"
import Allocation from "../Allocation/Allocation";
import AccountControll from "../Account/AccountControll";
import Account from "../Account/Account";
import useParseToken from "../../hooks/useParseToken";
import useUpdateToken from "../../hooks/useUpdateToken";
import useRedirectionRefreshToken from "../../hooks/useRedirectionRefreshToken";
import api from "../../api/helpAxios";
import Create from "../Create/Create";
import Slider from "./SliderImages";

import logo from "../../assets/Auth/LogoAuth.png"
import defaultUserImage from "../../assets/Account/user.png";
import { useContext, useEffect, useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import AuthContext from "../Context/AuthProvider";

const Home = () => {
    const [tab, setTab] = useState(null);
    const [userImage, setUserImage] = useState(null);
    const [login, setLogin] = useState("");

    const [profileIsOpen, setProfileIsOpen] = useState(false);
    const menuRef = useRef(null);

    const navigate = useNavigate();
    const {auth, setAuth} = useContext(AuthContext);

    const SignOut = (e) => {
        e.preventDefault();
        setAuth({});
        localStorage.removeItem("token");
        navigate("/Auth")
    } 

    const handleClickOutside = (event) => {
        if (menuRef.current && !menuRef.current.contains(event.target)) {
            setProfileIsOpen(false);
        }
    };

    const GetHomeProfile = async () => {
        try {
            const token = localStorage.getItem('token');
            const { id, login, role } = useParseToken(token ?? "");

            const response = await api.get("/Profile/GetHomeProfile", {
                withCredentials: true,
                params: {
                    idUser: id
                },
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token ?? ""}`
                }
            });

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
                return;
            }
            console.log(response);
            setLogin(response.data.data.login);
            setUserImage(response.data.data.image);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetAllAllocationRequest() },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    useEffect(() => {
        console.log(auth);
    }, [auth]);

    useEffect(() => {
        const fatchHomeProfile = async () => { await GetHomeProfile() };

        const token = localStorage.getItem("token");
        setAuth(useParseToken(token));

        fatchHomeProfile();
    }, []);

    return (
        <main className={styles.main} onClick={handleClickOutside}>
            <header className={styles.header}>
                <section className={styles.imageContainer}>
                    <a href="/Home"><img src={logo} alt="logo" height={60} width={60} /></a>
                </section>
                <section className={styles.navigationHeader}>
                    {auth.role != "User" && auth.role != "Organization" && (
                        <ul>
                            {auth.role == "Admin" && (<li onClick={() => { setTab(<AccountControll />) }}>Пользователи</li>)}
                            {auth.role == "Admin" && (<li onClick={() => { setTab(<Create SetTab={setTab}/>) }}>Создание</li>)}
                            <li onClick={() => { setTab(<Allocation />) }}>Распределение</li>
                        </ul>
                    )}
                    
                </section>
                <section className={styles.userContainer}>
                    <div className={styles.userImageContainer} onClick={() => {setProfileIsOpen(!profileIsOpen)}}>
                        <img src={userImage ?? defaultUserImage} alt="userlogo" height={55} width={55} />
                    </div>
                    <p>{login}</p>
                    {
                        profileIsOpen && (
                            <div className={styles.userMenu} ref={menuRef}>
                                <Link to="/Profile">Профиль</Link>
                                <button type="button" onClick={(e) => {SignOut(e)}}>Выйти</button>
                            </div>
                        )
                    }
                </section>
            </header>
            <div>
                {tab}
            </div>
            <footer className={styles.footer}>

            </footer>
        </main>
    )
}

export default Home;