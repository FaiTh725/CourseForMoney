
import { useContext, useEffect, useState } from "react";
import styles from "./Profile.module.css"
import useParseToken from "../../hooks/useParseToken";
import api from "../../api/helpAxios";
import { useNavigate } from "react-router-dom";
import useUpdateToken from "../../hooks/useUpdateToken";
import useRedirectionRefreshToken from "../../hooks/useRedirectionRefreshToken";
import AuthContext from "../Context/AuthProvider";

const Profile = () => {
    const [idUser, setIdUser] = useState(0);
    const [login, setLogin] = useState("");
    const [role, setRole] = useState(0);
    const [selectedGroup, setSelectedGroup] = useState(0);
    const [fullName, setFullName] = useState("");
    const [averageScore, setAverageScore] = useState(0);
    const [adressStudent, setAdressstudent] = useState("");
    const [isMaried, setIsMaried] = useState(false);
    const [isExtendedFamily, setIsExtendFamily] = useState(false);
    const [nameOrganization, setNameOrganization] = useState("");
    const [contacts, setContacts] = useState("");
    const [idAllocationRequest, setIdAllocationRequest] = useState();
    const [nameAdressAllocationRequest, setNameAdressAllocationRequest] = useState();
    const [countPlace, setCountPlace] = useState();

    const navigate = useNavigate();
    const { auth, setAuth } = useContext(AuthContext);
    const roles = {
        0: "Студент",
        1: "Глава кафедры",
        2: "Админ",
        3: "Организация"
    }

    const AddAllocationRequest = async (e) => {
        e.preventDefault();

        try {
            const token = localStorage.getItem("token");

            const response = await api.post("/Profile/AddAllocationRequest", {
                idUser: idUser,
                nameOrganization: nameOrganization,
                adressAllocationRequest: nameAdressAllocationRequest,
                countPlace: countPlace
            }, {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token == null ? "" : token}`
            });

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
                return;
            }

            console.log(response);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { AddAllocationRequest(e) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    const GetUserInfo = async () => {
        try {
            const token = localStorage.getItem("token");

            const { id, login, role } = useParseToken(token);

            const response = await api.get("/Account/GetUser", {
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token == null ? "" : token}`
                },
                params: {
                    idUser: id
                }
            });

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
                return;
            }

            console.log(response);

            setIdUser(response.data.data.id);
            setLogin(response.data.data.login);
            setRole(response.data.data.role);

            if (response.data.data.role == 0) {
                setSelectedGroup(response.data.data.group);
                setFullName(response.data.data.fullName);
                setAverageScore(response.data.data.averageScore);
                setAdressstudent(response.data.data.adressStudent);
                setIsMaried(response.data.data.isMarried);
                setIsExtendFamily(response.data.data.extendedFamily);
            }
            else if (response.data.data.role == 3) {
                setNameOrganization(response.data.data.nameOrganization);
                setContacts(response.data.data.contacts);
                setIdAllocationRequest(response.data.data.idAllocationRequest);
                setNameAdressAllocationRequest(response.data.data.nameAdressAllocationRequest);
                setCountPlace(response.data.data.countPlace);
            }
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetAllUsers() },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    };

    useEffect(() => {
        const fatchUserInfo = async () => { await GetUserInfo() };

        fatchUserInfo();
    }, []);

    return (
        <main className={styles.main}>
            <header className={styles.header}>
                <h1>Профиль</h1>
            </header>
            <section className={styles.profileContainer}>
                <div className={styles.profileImage}>
                    <img src="" alt="user Image" />
                </div>
                <div className={styles.mainInfo}>
                    <div className={styles.viewData}>
                        <label>Логин</label>
                        <h3>{login}</h3>
                    </div>
                    <div className={styles.viewData}>
                        <label>Статус</label>
                        <h3>{roles[role]}</h3>
                    </div>
                </div>
            </section>
            {
                role == 0 && (
                    <section className={styles.extentionInfo}>
                        <div className={styles.viewData}>
                            <label>Группа</label>
                            <h3></h3>
                        </div>
                        <div className={styles.viewData}>
                            <label>Полное имя</label>
                            <h3>{fullName}</h3>
                        </div>
                        <div className={styles.viewData}>
                            <label>Средний балл</label>
                            <h3>{averageScore}</h3>
                        </div>
                        <div className={styles.viewData}>
                            <label>Адрес</label>
                            <h3>{adressStudent}</h3>
                        </div>
                        <div className={styles.viewData}>
                            <label>Женат</label>
                            <h3>{isMaried}</h3>
                        </div>
                        <div className={styles.viewData}>
                            <label>Многодетная семья</label>
                            <h3>{isExtendedFamily}</h3>
                        </div>
                    </section>
                )
            }
            {role == 3 && (
                <section className={styles.extentionInfo}>
                    <div className={styles.organizationInfo}>
                        <div className={styles.inputData}>
                            <label>Название</label>
                            <input type="text" defaultValue={nameOrganization} />
                        </div>
                        <div className={styles.inputDataData}>
                            <label>Контакты</label>
                            <input type="text" defaultValue={contacts} />
                        </div>
                        <form className={styles.organizationRequest}>
                            <div className={styles.inputData}>
                                <label>Адрес</label>
                                <input onChange={(e) => { setNameAdressAllocationRequest(e.target.value) }} defaultValue={nameAdressAllocationRequest} type="text" placeholder="Адрес" />
                            </div>
                            <div className={styles.inputData}>
                                <label>Количество мест</label>
                                <input onChange={(e) => { setCountPlace(e.target.value) }} defaultValue={countPlace} type="text" placeholder="количество мест" />
                            </div>
                            {
                                idAllocationRequest == null && (
                                    <div >
                                        <button type="submit">Добавить запрос</button>
                                    </div>
                                )
                            }
                            {
                                idAllocationRequest != null && (
                                    <div >
                                        <button type="submit">Удалить запрос</button>
                                    </div>
                                )
                            }
                        </form>
                    </div>

                </section>

            )}
        </main>
    )
}

export default Profile;

