
import { useContext, useEffect, useRef, useState } from "react";
import styles from "./Profile.module.css"
import useParseToken from "../../hooks/useParseToken";
import api from "../../api/helpAxios";
import { Link, useNavigate } from "react-router-dom";
import useUpdateToken from "../../hooks/useUpdateToken";
import useRedirectionRefreshToken from "../../hooks/useRedirectionRefreshToken";
import AuthContext from "../Context/AuthProvider";
import MiniModal from "../Modal/MiniModal";


import defaultUserImage from "../../assets/Account/user.png";
import arrowLeft from "../../assets/Profile/ArrowLeft.svg"
import RequestOrganization from "./RequestOrganization";

// какаето хуйня если добавлять сначало файл потом поля то нихуя не работате
const Profile = () => {
    const [idUser, setIdUser] = useState(0);
    const [login, setLogin] = useState("");
    const [role, setRole] = useState(null);
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

    const [idOrganization, setIdOrganization] = useState(null);
    const [nameOrganization, setNameOrganization] = useState("");
    const [contacts, setContacts] = useState("");
    const [requests, setRequests] = useState([]);

    const [activeMiniModal, setActiveMiniModal] = useState(false);
    const [miniMidalMessage, setMiniModalMessage] = useState("");

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
            setMiniModalMessage("Обновили профиль");
            setActiveMiniModal(true);
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

    const ChangeRequest = async (e, idRequest, adress, specialist, countPlace) => {
        e.preventDefault();

        try {
            const token = localStorage.getItem("token");

            const response = await api.patch("/Profile/ChangeRequest", {
                idRequest: idRequest,
                adress: adress,
                countPlace: countPlace,
                specialist: specialist
            }, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token == null ? "" : token}`
                }
            });

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
                return;
            }

            setMiniModalMessage("Обновили заявку");
            setActiveMiniModal(true);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { ChangeRequest(e, idRequest, adress, specialist, countPlace) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    const DeleteAllocationRequest = async (e, idRequestAllocation, idOrganization) => {
        e.preventDefault();

        try {
            const token = localStorage.getItem("token");

            const response = await api.delete("/Profile/DeleteAllocationRequest", {
                data: {
                    idRequest: idRequestAllocation,
                    idOrganization: idOrganization,
                },
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token == null ? "" : token}`
                }
            });

            if (response.data.statusCode != 0) {
                alert(response.data.description);
                return;
            }

            console.log(response);
            var updateRequests = requests.filter(request => request.idRequest !== idRequestAllocation);

            setRequests(updateRequests);
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

    const AddOrderFileRequest = async (idRequest, orderFile) => {
        try {
            var token = localStorage.getItem("token");

            var data = new FormData();

            data.append("idAllocationRequest", idRequest);
            data.append("orderRequest", orderFile);

            var response = await api.post("/Profile/AddOrderFileToRequest",
                data,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        'Authorization': `Bearer ${token == null ? "" : token}`
                    }
                });

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
            }

            const updatedData = requests.map(request => {
                if (request.idRequest == idRequest) {
                    return { ...request, urlOrderFile: response.data.data.urlOrderFile }
                }
                return request;
            });

            setRequests(updatedData);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { AddOrderFileRequest(idRequest) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    const AddAllocationRequest = async (e, idOrganization, adress, specialist, countFreePlace, orderFile) => {
        e.preventDefault();

        if (adress == "" || countFreePlace == "" || specialist == "") {
            alert("введите данные(сделать модальным окном)")
            return;
        }

        if (orderFile == null) {
            alert("загрузите файл(сделать модальным окном)")
            return;
        }

        try {
            const token = localStorage.getItem("token");

            const response = await api.post("/Profile/AddAllocationRequest",
                {
                    idOrganization: idOrganization,
                    adress: adress,
                    specialist: specialist,
                    countFreePlace: countFreePlace
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

            await AddOrderFileRequest(response.data.data.idRequest, orderFile);

            setRequests(prevRequest => [...prevRequest, {
                idRequest: response.data.data.idRequest,
                countPlace: response.data.data.countPlace,
                specialist: response.data.data.specialist,
                nameRequest: response.data.data.nameRequest,
                urlOrderFile: response.data.data.urlOrderFile
            }]);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { AddAllocationRequest(e, adress, specialist, countFreePlace) },
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

    const GetRequestOrganization = async (idOrganization) => {
        try {
            const token = localStorage.getItem("token");

            const response = await api.get("/Profile/GetAllRequestsOrganazation", {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token == null ? "" : token}`
                },
                params: {
                    idOrganization: idOrganization
                }
            });

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
                return;
            }

            setRequests(response.data.data);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetRequestOrganization(idOrganization) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    useEffect(() => {
        if (role == 3) {
            GetOrganizationInfo();

        }
        else if (role == 0) {
            GetStudentInfo();
        }
    }, [role]);

    const GetStudentInfo = async () => {
        try {
            var token = localStorage.getItem("token");

            const { id, login, role } = useParseToken(token);

            var response = await api.get("/Profile/GetStudentPofile", {
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


        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetStudentInfo() },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    const GetOrganizationInfo = async () => {
        try {
            var token = localStorage.getItem("token");

            const { id, login, role } = useParseToken(token);

            var response = await api.get("/Profile/GetOrganizationPofile", {
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
            setIdOrganization(response.data.data.idOrganization);
            setNameOrganization(response.data.data.nameOrganization);
            setContacts(response.data.data.contacts);
            setRequests(response.data.data.requests);

        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetOrganizationInfo() },
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

            const response = await api.get("/Profile/GetUserPofile", {
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
            <MiniModal active={activeMiniModal} setActive={setActiveMiniModal}>
                <p>{miniMidalMessage}</p>
            </MiniModal>
            <header className={styles.header}>
                <h1>Профиль</h1>
                <Link className={styles.backHomeBtn} to="/Home">
                    <img src={arrowLeft} height={25} alt="arrowleft" />
                    <p>назад</p>
                </Link>
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
                    <section className={styles.organizationRequests}>
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
                                <RequestOrganization type={"add"}
                                    idOrganization={idOrganization}
                                    AddAllocationRequest={AddAllocationRequest}
                                    DeleteAllocationRequest={DeleteAllocationRequest}
                                    AddOrderFileRequest={AddAllocationRequest}
                                    request={null} />
                            </div>

                        </section>
                        <section className={styles.listRequests}>
                            {
                                requests.map((request, index) => (
                                    <RequestOrganization key={index} type="view" idOrganization={idOrganization}
                                        DeleteAllocationRequest={DeleteAllocationRequest} 
                                        AddAllocationRequest={AddAllocationRequest}
                                        ChangeRequest = {ChangeRequest}
                                        request={request} />

                                ))
                            }
                        </section>
                    </section>

                )}
            </section>

        </main>
    )
}

export default Profile;

