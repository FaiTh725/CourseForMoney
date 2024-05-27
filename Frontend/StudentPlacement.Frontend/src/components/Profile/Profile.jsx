
import { useContext, useEffect, useRef, useState } from "react";
import styles from "./Profile.module.css"
import useParseToken from "../../hooks/useParseToken";
import api from "../../api/helpAxios";
import { useNavigate } from "react-router-dom";
import useUpdateToken from "../../hooks/useUpdateToken";
import useRedirectionRefreshToken from "../../hooks/useRedirectionRefreshToken";
import AuthContext from "../Context/AuthProvider";

import defaultUserImage from "../../assets/Account/user.png";
import deleteCross from "../../assets/Account/delete_cross.png"
import plus from "../../assets/Account/plus.png"

// валидацию на ввод
// возле полей женат и многодетная добавить значки да или нет а то там ничего
// в зависимости от запроса отправлять запрос на получения или организации или студента или ничего
const Profile = () => {
    const [idUser, setIdUser] = useState(0);
    const [login, setLogin] = useState("");
    const [role, setRole] = useState(0);
    const [group, setGroup] = useState("");
    const [image, setImage] = useState(null);
    const [email, setEmail] = useState("");

    const [fullName, setFullName] = useState("");
    const [averageScore, setAverageScore] = useState(0);
    const [adressStudent, setAdressstudent] = useState("");
    const [isMaried, setIsMaried] = useState(false);
    const [isExtendedFamily, setIsExtendFamily] = useState(false);
    const [idStudentRequest, setIdStudentRequest] = useState(null);
    const [adressRequest, setAdressRequest] = useState("");
    const [organizationNameRequest, setOrganizationNameRequest] = useState("");
    const [contactRequest, setContactsRequest] = useState("");

    const [nameOrganization, setNameOrganization] = useState("");
    const [contacts, setContacts] = useState("");
    const [idAllocationRequest, setIdAllocationRequest] = useState();
    const [nameAdressAllocationRequest, setNameAdressAllocationRequest] = useState();
    const [countPlace, setCountPlace] = useState();



    const refRequest = useRef(null);
    const refCountPlace = useRef(null);

    const navigate = useNavigate();
    const { auth, setAuth } = useContext(AuthContext);
    const roles = {
        0: "Студент",
        1: "Глава кафедры",
        2: "Админ",
        3: "Организация"
    }

    const ChangeProfile = async () => {
        try {
            const token = localStorage.getItem("token");

            const response = await api.patch("/Profile/ChangeProfile", {
                loginUser: login,
                organizationName: nameOrganization,
                contact: contacts,
                allocationId: idAllocationRequest,
                adress: nameAdressAllocationRequest,
                countPlace: countPlace
            }, {
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token == null ? "" : token}`
                }
            });

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
            }

            console.log(response);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { ChangeProfile() },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    const DeleteAllocationRequest = async (e) => {
        e.preventDefault();

        try {
            const token = localStorage.getItem("token");

            const response = await api.delete("/Profile/DeleteAllocationRequest", {
                data: {
                    idRequest: idAllocationRequest,
                    organizationName: nameOrganization,
                    loginUser: login
                },
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token == null ? "" : token}`
                }
            });

            console.log(response);
            setNameAdressAllocationRequest();
            setCountPlace();
            setIdAllocationRequest();
            refCountPlace.current.value = "";
            refRequest.current.value = "";
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { DeleteAllocationRequest(e) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    const AddAllocationRequest = async (e) => {
        e.preventDefault();

        try {
            const token = localStorage.getItem("token");

            const response = await api.post("/Profile/AddAllocationRequest",
                {
                    id: idUser,
                    organizationName: nameOrganization,
                    allocationRequestAdress: nameAdressAllocationRequest,
                    countSpace: countPlace
                },
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token == null ? "" : token}`
                    }
                });

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
                return;
            }

            console.log(response);
            setIdAllocationRequest(response.data.data.idAllocatinRequest);
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

    const GetUserRequest = async () => {
        try {
            const token = localStorage.getItem("token");

            const { id, login, role } = useParseToken(token);

            if (role != "User") {
                return;
            }

            const response = await api.get('/Profile/GetStudentRequest',
                {
                    params: {
                        idUser: id
                    },
                    withCredentials: true,
                    headers: {
                        'Authorization': `Bearer ${token == null ? "" : token}`
                    }
                }
            );

            console.log(response);
            setIdStudentRequest(response.data.data.idRequest);
            setOrganizationNameRequest(response.data.data.requestNameOrganization);
            setAdressRequest(response.data.data.requestAdressRequest);
            setContactsRequest(response.data.data.requestContacts);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetUserRequest() },
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
            setImage(response.data.data.image);
            setEmail(response.data.data.email);

            if (response.data.data.role == 0) {
                setGroup(response.data.data.group);
                setFullName(response.data.data.fullName);
                setAverageScore(response.data.data.averageScore);
                setAdressstudent(response.data.data.adressStudent);
                setIsMaried(response.data.data.isMarried);
                setIsExtendFamily(response.data.data.extendedFamily);
                setGroup(response.data.data.groupName);
            }
            else if (response.data.data.role == 3) {
                setNameOrganization(response.data.data.nameOrganization);
                setContacts(response.data.data.contacts);
                setIdAllocationRequest(response.data.data.idAllocationRequest);
                setNameAdressAllocationRequest(response.data.data.nameAdressAllocationrequestRequest);
                setCountPlace(response.data.data.countPlace);
            }
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetUserInfo() },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    };

    useEffect(() => {
        const fatchUserInfo = async () => { await GetUserInfo() };
        const fatchUserRequest = async () => { await GetUserRequest() };

        fatchUserInfo();
        fatchUserRequest();
    }, []);

    return (
        <main className={styles.main}>
            <header className={styles.header}>
                <h1>Профиль</h1>
            </header>
            <section className={styles.profileContainer}>
                <div className={styles.profileImage}>
                    <img src={image == "" || image == null ? defaultUserImage : image} alt="user Image" height={140} width={140} />
                </div>
                <div className={styles.mainInfo}>
                    <div className={styles.viewData}>
                        <label>Логин</label>
                        <h3>{login}</h3>
                    </div>
                    <div className={styles.viewData}>
                        <label>Почта</label>
                        <h3>{email}</h3>
                    </div>
                    <div className={styles.viewData}>
                        <label>Статус</label>
                        <h3>{roles[role]}</h3>
                    </div>
                </div>
                {
                    role == 0 && (
                        <div>
                            <section className={styles.extentionInfo}>
                                <div className={styles.viewData}>
                                    <label>Группа</label>
                                    <h3>{group}</h3>
                                </div>
                                <div className={styles.viewData}>
                                    <label className={styles.test}>Полное имя</label>
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
                            {
                                idStudentRequest != null && (
                                    <section className={styles.extentionInfo}>
                                        <div className={styles.viewData}>
                                            <label>Адрес заявки</label>
                                            <h3>{adressRequest}</h3>
                                        </div>
                                        <div className={styles.viewData}>
                                            <label>Название организации</label>
                                            <h3>{organizationNameRequest}</h3>
                                        </div>
                                        <div className={styles.viewData}>
                                            <label>Контакты</label>
                                            <h3>{contactRequest}</h3>
                                        </div>
                                    </section>
                                )
                            }
                            {
                                idStudentRequest == null && (
                                    <div className={styles.noRequestMessgae}>
                                        <h3>Вас пока не распределили</h3>
                                    </div>
                                )
                            }

                        </div>
                    )
                }
                {role == 3 && (
                    <section className={styles.extentionInfo}>
                        <div className={styles.organizationInfo}>
                            <div className={styles.inputData}>
                                <label>Название</label>
                                <input onBlur={() => { ChangeProfile() }} type="text" onChange={(e) => { setNameOrganization(e.target.value) }} defaultValue={nameOrganization} />
                            </div>
                            <div className={styles.inputData}>
                                <label>Контакты</label>
                                <input onBlur={() => { ChangeProfile() }} type="text" onChange={(e) => { setContacts(e.target.value) }} defaultValue={contacts} />
                            </div>
                            <form className={styles.organizationRequest}>
                                <div className={styles.inputData}>
                                    <label>Адрес</label>
                                    <input onBlur={() => { ChangeProfile() }} ref={refRequest} onChange={(e) => { setNameAdressAllocationRequest(e.target.value) }} defaultValue={nameAdressAllocationRequest} type="text" placeholder="Адрес" />
                                </div>
                                <div className={styles.inputData}>
                                    <label>Количество мест</label>
                                    <input onBlur={() => { ChangeProfile() }} ref={refCountPlace} onChange={(e) => { setCountPlace(e.target.value) }} defaultValue={countPlace} type="text" placeholder="количество мест" />
                                </div>
                                {
                                    idAllocationRequest == null && (
                                        <div className={styles.btnContainer} onClick={(e) => { AddAllocationRequest(e) }}>
                                            <button className={styles.addBtn} type="submit">
                                                <p>Добавить запрос</p>
                                                <img src={plus} alt="delete profile" height={35} />
                                            </button>
                                        </div>
                                    )
                                }
                                {
                                    idAllocationRequest != null && (
                                        <div className={styles.btnContainer} onClick={(e) => { DeleteAllocationRequest(e) }}>
                                            <button className={styles.deleteBtn} type="submit">
                                                <p>Удалить запрос</p>
                                                <img src={deleteCross} alt="delete profile" height={35} />
                                            </button>
                                        </div>
                                    )
                                }
                            </form>
                        </div>

                    </section>

                )}
            </section>

        </main>
    )
}

export default Profile;

