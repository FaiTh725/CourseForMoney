import { useContext, useEffect, useState } from "react"
import styles from "./AccountControll.module.css"
import api from "../../api/helpAxios";
import { useNavigate } from "react-router-dom";
import useRedirectionRefreshToken from "../../hooks/useRedirectionRefreshToken";
import useParseToken from "../../hooks/useParseToken";
import useUpdateToken from "../../hooks/useUpdateToken";
import AuthContext from "../Context/AuthProvider";

const AccountControll = () => {
    const [users, setUsers] = useState([]);
    const [usersView, setUsersView] = useState([]);
    const [searchString, setSearchString] = useState("");
    const navigate = useNavigate();
    const { auth, setAuth } = useContext(AuthContext);
    const [allGroups, setAllGroups] = useState([]);
    
    const DeleteUser = async (e, idUser) => {
        e.preventDefault();
        try {
            const token = localStorage.getItem("token");
            const { id, login, role } = useParseToken(token == null ? "" : token);

            if (id == idUser) {
                alert("Удаление самого себя (потом сделать как всплыющие окно)");
                return;
            }


            const response = await api.delete("/Account/DeleteUser", {
                data: {
                    idUser: idUser
                },
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token == null ? "" : token}`
                }
            });

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
                return;
            };

            const newUsers = users.filter(x => x.id != idUser);
            console.log(response);
            setUsers(newUsers);
            setUsersView(newUsers);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { DeleteUser(e, idUser) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    const GetAllGroup = async () => {
        try {

            const token = localStorage.getItem("token");

            var response = await api.get("/Account/GetStudentSetting", {
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token == null ? "" : token}`
                }
            });

            console.log(response.data.description);

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
                return;
            }

            setAllGroups(response.data.data);
        }
        catch (error) {
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetAllGroup() },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    const GetAllUsers = async () => {
        try {
            const token = localStorage.getItem("token");

            const response = await api.get("/Account/GetAllUsers", {
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token == null ? "" : token}`
                }
            });

            console.log(response);
            setUsers(response.data.data);
            setUsersView(response.data.data);
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
    }

    const ChangeInfoUser = async (idUser, loginUser, passwordUser, roleUser, selectedGroup,
        fullName, averageScore, adressStudent,
        isMaried, isExtendedFamily,
        nameOrganization, contacts) => {
        try {
            const token = localStorage.getItem("token");
            const { id, login, role } = useParseToken(token);

            if (id == idUser && role == roleUser) {
                alert("я вам запрещаю изменять свою роль");
                return;
            }

            var data = {
                id: idUser,
                login: loginUser,
                password: passwordUser,
                role: roleUser,
                group: selectedGroup,
                fullName: "",
                averageScore: 0,
                adressStudent: "",
                isMarried: false,
                extendedFamily: false,
                nameOrganization: "",
                contacts: ""
            };

            console.log(data);

            if (roleUser == 0) {
                data = {
                    ...data,
                    group: selectedGroup,
                    fullName: fullName,
                    averageScore: averageScore,
                    adressStudent: adressStudent,
                    isMarried: isMaried,
                    extendedFamily: isExtendedFamily
                }
            }
            else if (roleUser == 3) {
                data = {
                    ...data,
                    nameOrganization: nameOrganization,
                    contacts: contacts
                }
            }

            const response = await api.patch("/Account/ChangeUser",
                data,
                {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token == null ? "" : token}`
                    }
                });

            console.log(response);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => {
                    ChangeInfoUser(idUser, loginUser, passwordUser, roleUser, group,
                        fullName, averageScore, adressStudent,
                        isMaried, isExtendedFamily,
                        nameOrganization, contacts,
                        DeleteUser, allGroups)
                },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    useEffect(() => {
        setUsersView(users.filter(user => user.login.startsWith(searchString)));
    }, [searchString]);

    useEffect(() => {
        const fatchAllUsers = async () => { await GetAllUsers() };
        const fatchAllGroups = async () => { await GetAllGroup() };

        fatchAllUsers();
        fatchAllGroups();
    }, []);

    return (
        <main className={styles.main}>
            <header className={styles.header}>
                <img src="" alt="" />
                <h1>Управление аккаунтами</h1>
            </header>
            <section className={styles.searchSection}>
                <div className={styles.searchInner}>
                    <img src="" alt="search" />
                    <input type="text" onChange={(e) => { setSearchString(e.target.value) }} placeholder="поиск по логину" />
                </div>
            </section>
            <section className={styles.accountsSection}>
                {usersView.map(user => (
                    <CardUser key={user.id} id={user.id} login={user.login} password={user.password} role={user.role}
                        group={user.group} fullName={user.fullName} adressStudent={user.adressStudent} averageScore={user.averageScore} isMaried={user.isMarried} isExtendedFamily={user.extendedFamily}
                        nameOrganization={user.nameOrganization} contacts={user.contacts}
                        DeleteUser={DeleteUser} allGroups={allGroups} ChangeInfoUser={ChangeInfoUser} />
                ))}
                {/* <CardUser role={3}/> */}
            </section>
        </main>
    )
}

const CardUser = ({ id, login, password, role, group,
    fullName, averageScore, adressStudent,
    isMaried, isExtendedFamily,
    nameOrganization, contacts,
    DeleteUser, allGroups, ChangeInfoUser }) => {
    const [loginCur, setLogin] = useState(login);
    const [passwordCur, setPassword] = useState(password);
    const [roleCur, setRole] = useState(role);
    const [selectedGroupCur, setSelectedGroup] = useState(group);
    const [fullNameCur, setFullName] = useState(fullName);
    const [averageScoreCur, setAverageScore] = useState(averageScore);
    const [adressStudentCur, setAdressstudent] = useState(adressStudent);
    const [isMariedCur, setIsMaried] = useState(isMaried);
    const [isExtendedFamilyCur, setIsExtendFamily] = useState(isExtendedFamily);
    const [nameOrganizationCur, setNameOrganization] = useState(nameOrganization);
    const [contactsCur, setContacts] = useState(contacts);

    const [allGroupsCur, setAllGroupsCur] = useState(allGroups);

    const [showExtentionInfo, setShowExtentionInfo] = useState(false);

    const ChangeInfo = async () => {
        await ChangeInfoUser(id, loginCur,
            passwordCur,
            roleCur,
            selectedGroupCur,
            fullNameCur,
            averageScoreCur,
            adressStudentCur,
            isMariedCur,
            isExtendedFamilyCur,
            nameOrganizationCur,
            contactsCur,);
    }

    const roles = {
        0: "Студент",
        1: "Глава кафедры",
        2: "Админ",
        3: "Организация"
    }

    return (
        <div className={styles.cardUserMain}>
            <div className={styles.mainData}>
                <img src="" alt="logo user" />
                <div className={styles.data}>
                    <div className={styles.deleteBtnContainer}>
                        <button type="button" onClick={(e) => { DeleteUser(e, id) }}>
                            <img src="" alt="delete profile" />
                        </button>
                    </div>
                    <div className={styles.inputData}>
                        <label >Логин</label>
                        <input type="text" placeholder="логин" onBlur={(e) => { e.preventDefault(); ChangeInfo() }} defaultValue={loginCur} onChange={(e) => { setLogin(e.target.value) }} />
                    </div>
                    <div className={styles.inputData}>
                        <label >Пароль</label>
                        <input type="text" placeholder="пароль" onBlur={(e) => { e.preventDefault(); ChangeInfo() }} defaultValue={passwordCur} onChange={(e) => { setPassword(e.target.value) }} />
                    </div>
                    <div className={styles.inputData}>
                        <label >Роль</label>
                        <input type="text" disabled={true} defaultValue={roles[roleCur]} />
                    </div>
                </div>
            </div>
            {
                (roleCur == 0 || roleCur == 3) && (
                    <div>
                        <div className={styles.showExtentionInfoBtnContainer}>
                            <button onClick={(e) => { e.preventDefault(); setShowExtentionInfo(!showExtentionInfo) }}>
                                <p>доп информация</p>
                                <img src="" alt="arrow down" />
                            </button>
                        </div>
                        {showExtentionInfo == true && (
                            <div className={styles.extentionInfo}>
                                {role == 0 && (
                                    <div className={styles.settingStudent}>
                                        <div className={styles.inputData}>
                                            <label >Группа</label>
                                            <select defaultValue={selectedGroupCur} onBlur={(e) => { e.preventDefault(); ChangeInfo() }} onChange={(e) => { setSelectedGroup(e.target.value) }}>
                                                {
                                                    allGroupsCur.map(group => (
                                                        <option key={group.id} value={group.id}>{group.name}</option>
                                                    ))
                                                }
                                            </select>
                                        </div>
                                        <div className={styles.inputData}>
                                            <label >Полное имя</label>
                                            <input type="text" placeholder="полное имя" onBlur={(e) => { e.preventDefault(); ChangeInfo() }} defaultValue={fullNameCur} onChange={(e) => { setFullName(e.target.value) }} />
                                        </div>
                                        <div className={styles.inputData}>
                                            <label >Средний балл</label>
                                            <input type="text" placeholder="средний балл" onBlur={(e) => { e.preventDefault(); ChangeInfo() }} defaultValue={averageScoreCur} onChange={(e) => { setAverageScore(e.target.value) }} />
                                        </div>
                                        <div className={styles.inputData}>
                                            <label >Адрес</label>
                                            <input type="text" placeholder="адрес" onBlur={(e) => { e.preventDefault(); ChangeInfo() }} defaultValue={adressStudentCur} onChange={(e) => { setAdressstudent(e.target.value) }} />
                                        </div>
                                        <div className={styles.inputData}>
                                            <label >Женат</label>
                                            <input type="checkbox" checked={isMariedCur} onBlur={(e) => { e.preventDefault(); ChangeInfo() }} onChange={(e) => { setIsMaried(!isMariedCur) }} />
                                        </div>
                                        <div className={styles.inputData}>
                                            <label >Многодетная семья</label>
                                            <input type="checkbox" checked={isExtendedFamilyCur} onBlur={(e) => { e.preventDefault(); ChangeInfo() }} onChange={(e) => { setIsExtendFamily(!isExtendedFamilyCur) }} />
                                        </div>
                                    </div>
                                )}
                                {role == 3 && (
                                    <div className={styles.settingOrganization}>
                                        <div className={styles.inputData}>
                                            <label >Название</label>
                                            <input type="text" placeholder="адрес" onBlur={(e) => { e.preventDefault(); ChangeInfo() }} defaultValue={nameOrganizationCur} onChange={(e) => { setNameOrganization(e.target.value) }} />
                                        </div>
                                        <div className={styles.inputData}>
                                            <label >Контакты</label>
                                            <input type="text" placeholder="адрес" onBlur={(e) => { e.preventDefault(); ChangeInfo() }} defaultValue={contactsCur} onChange={(e) => { setContacts(e.target.value) }} />
                                        </div>
                                    </div>
                                )}
                            </div>
                        )}
                    </div>
                )
            }


        </div>
    )
}

export default AccountControll;