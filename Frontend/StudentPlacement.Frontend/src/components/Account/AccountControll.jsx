import { useContext, useEffect, useRef, useState } from "react"
import styles from "./AccountControll.module.css"
import api from "../../api/helpAxios";
import { useNavigate } from "react-router-dom";
import useRedirectionRefreshToken from "../../hooks/useRedirectionRefreshToken";
import useParseToken from "../../hooks/useParseToken";
import useUpdateToken from "../../hooks/useUpdateToken";
import AuthContext from "../Context/AuthProvider";
import useImgToString from "../../hooks/useImgToString";

import findImg from "../../assets/Account/find.png";
import deleteCross from "../../assets/Account/delete_cross.png";
import arrowDown from "../../assets/Account/arrow_down.png";
import userImageExample from "../../assets/Account/user.png";
import circleGray from "../../assets/Account/circleGray.png"
import circleGreen from "../../assets/Account/circleGree.png"

// сортировку по ролям
// валидацию на ввод
// при изменении была шляпа хз пофиксил ли
// сортировка
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

    const ChangeInfoUser = async (idUser, loginUser, passwordUser,
        email, roleUser,
        image, selectedGroup,
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
                email: email,
                role: roleUser,
                image: image,
                group: 0,
                fullName: "",
                averageScore: 0,
                adressStudent: "",
                isMarried: false,
                extendedFamily: false,
                organizationName: "",
                contact: ""
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
                    organizationName: nameOrganization,
                    contact: contacts
                }
            }

            const formData = new FormData();
            formData.append("id", idUser);
            formData.append("login", data.login);
            formData.append("password", data.password);
            formData.append("email", data.email);
            formData.append("role", data.selectedGroup);
            formData.append("image", image);
            formData.append("group", data.group);
            formData.append("fullName", data.fullName);
            formData.append("averageScore", data.averageScore);
            formData.append("adressStudent", data.adressStudent);
            formData.append("isMarried", data.isMarried);
            formData.append("extendedFamily", data.extendedFamily);
            formData.append("nameOrganization", data.nameOrganization);
            formData.append("contacts", data.contact);

            const response = await api.patch("/Account/ChangeUser",
                data,
                {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        'Authorization': `Bearer ${token == null ? "" : token}`
                    }
                });

            console.log(response);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => {
                    ChangeInfoUser(idUser, loginUser, passwordUser, roleUser, image, group,
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
                    <img src={findImg} alt="search" height={40} />
                    <input type="text" onChange={(e) => { setSearchString(e.target.value) }} placeholder="поиск по логину" />
                </div>
            </section>
            <section className={styles.accountsSection}>
                {usersView.map(user => (
                    <CardUser key={user.id} id={user.id} image={user.image} login={user.login} password={user.password}
                        email={user.email} role={user.role}
                        group={user.group} fullName={user.fullName} adressStudent={user.adressStudent} averageScore={user.averageScore} isMaried={user.isMarried} isExtendedFamily={user.extendedFamily}
                        nameOrganization={user.nameOrganization} contacts={user.contacts}
                        DeleteUser={DeleteUser} allGroups={allGroups} ChangeInfoUser={ChangeInfoUser} />
                ))}
                {/* <CardUser role={3}/> */}
            </section>
        </main>
    )
}

const CardUser = ({ id, login, image, password,
    email, role, group,
    fullName, averageScore, adressStudent,
    isMaried, isExtendedFamily,
    nameOrganization, contacts,
    DeleteUser, allGroups, ChangeInfoUser }) => {
    const [imageCur, setImage] = useState(image);
    const [uploadFile, setUploadFile] = useState(null);
    const [loginCur, setLogin] = useState(login);
    const [passwordCur, setPassword] = useState(password);
    const [emailCur, setEmail] = useState(email);
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

    const imgBtn = useRef(null);

    //error messages
    const loginError = useRef(null);
    const passwordError = useRef(null);
    const averageScoreError = useRef(null);
    const emailError = useRef(null);


    const ChangeInfo = async () => {
        var flagError = false;
        if (loginCur.length < 4) {
            loginError.current.textContent = "Логин короче 4 символов";
            flagError = true;
        }
        if (passwordCur.length < 4) {
            passwordError.current.textContent = "Пароль короче 4 символов";
            flagError = true;
        }
        else if (!/['a-zA-Z']/.test(passwordCur) || !/['0-9']/.test(passwordCur)) {
            passwordError.current.textContent = "Пароль должен содержать 1 цифру и 1 букву";
            flagError = true;
        }
        if (role == 0 && !(Number.parseFloat(averageScore) >= 0 && Number.parseFloat(averageScore) <= 10)) {
            averageScoreError.current.textContent = "Средний балл должен быть в пределах от 0 до 4";
            flagError = true;
        }
        if (emailCur.length == "" || !email.includes("@")) {
            emailError.current.textContent = "Неверная почта";
            flagError = true;
        }

        if (!flagError) {

            await ChangeInfoUser(id,
                loginCur,
                passwordCur,
                emailCur,
                roleCur,
                uploadFile,
                selectedGroupCur,
                fullNameCur,
                averageScoreCur,
                adressStudentCur,
                isMariedCur,
                isExtendedFamilyCur,
                nameOrganizationCur,
                contactsCur,);
        }
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
                <img onClick={() => { imgBtn.current.click() }} src={uploadFile == null ? imageCur == null ? userImageExample : imageCur : URL.createObjectURL(uploadFile)} alt="logo user" height={70} width={70} />
                <input onChange={(e) => { setUploadFile(e.target.files[0]) }} className={styles.inputFile} accept=".png" type="file" ref={imgBtn} />
                <div className={styles.data}>
                    <div className={styles.deleteBtnContainer}>
                        <button className={styles.deleteBtn} type="button" onClick={(e) => { DeleteUser(e, id) }}>
                            <p>Удалить</p>
                            <img src={deleteCross} alt="delete profile" height={35} />
                        </button>
                    </div>
                    <div className={styles.inputData}>
                        <label >Логин</label>
                        <input type="text" placeholder="логин" defaultValue={loginCur} onChange={(e) => { setLogin(e.target.value) }} />
                    </div>
                    <div className={styles.inputData}>
                        <label >Пароль</label>
                        <input type="text" placeholder="пароль" defaultValue={passwordCur} onChange={(e) => { setPassword(e.target.value) }} />
                    </div>
                    <div className={styles.inputData}>
                        <label >Почта</label>
                        <input type="text" placeholder="почта" defaultValue={emailCur} onChange={(e) => { setEmail(e.target.value) }} />
                    </div>
                    <div className={styles.inputData}>
                        <label >Роль</label>
                        <input type="text" disabled={true} defaultValue={roles[roleCur]} />
                    </div>
                    <div>
                        <button onClick={(e) => { e.preventDefault(); ChangeInfo() }}>сохранить</button>
                    </div>
                </div>
            </div>
            {
                (roleCur == 0 || roleCur == 3) && (
                    <div className={styles.extendedInfoContainer}>
                        <div className={styles.showExtentionInfoBtnContainer}>
                            <button className={styles.btnShowInfo} onClick={(e) => { e.preventDefault(); setShowExtentionInfo(!showExtentionInfo) }}>
                                <p>доп информация</p>
                                <img src={arrowDown} height={20} alt="arrow down" />
                            </button>
                        </div>
                        {showExtentionInfo == true && (
                            <div className={styles.extentionInfo}>
                                {role == 0 && (
                                    <div className={styles.settingStudent}>
                                        <div className={styles.inputDataExtend}>
                                            <label >Группа</label>
                                            <select defaultValue={selectedGroupCur} onChange={(e) => { setSelectedGroup(e.target.value) }}>
                                                {
                                                    allGroupsCur.map(group => (
                                                        <option key={group.id} value={group.id}>{group.name}</option>
                                                    ))
                                                }
                                            </select>
                                        </div>
                                        <div className={styles.inputDataExtend}>
                                            <label >Полное имя</label>
                                            <input type="text" placeholder="полное имя" defaultValue={fullNameCur} onChange={(e) => { setFullName(e.target.value) }} />
                                        </div>
                                        <div className={styles.inputDataExtend}>
                                            <label >Средний балл</label>
                                            <input type="text" placeholder="средний балл" defaultValue={averageScoreCur} onChange={(e) => { setAverageScore(e.target.value) }} />
                                        </div>
                                        <div className={styles.inputDataExtend}>
                                            <label >Адрес</label>
                                            <input type="text" placeholder="адрес" defaultValue={adressStudentCur} onChange={(e) => { setAdressstudent(e.target.value) }} />
                                        </div>
                                        <div className={styles.inputDataExtend}>
                                            <label >Женат</label>
                                            <button onClick={(e) => { e.preventDefault(); setIsMaried(!isMariedCur); }} type="button" className={isMariedCur == true ? styles.checkBoxChecked : styles.checkBox}>
                                                <img src={isMariedCur ? circleGreen : circleGray} alt="" height={20} width={20} />
                                            </button>
                                            {/* <input type="checkbox" checked={isMariedCur} onBlur={(e) => { e.preventDefault(); ChangeInfo() }} onChange={(e) => { setIsMaried(!isMariedCur) }} /> */}
                                        </div>
                                        <div className={styles.inputDataExtend}>
                                            <label >Многодетная семья</label>
                                            <button onClick={(e) => { e.preventDefault(); setIsExtendFamily(!isExtendedFamilyCur); }} type="button" className={isExtendedFamilyCur == true ? styles.checkBoxChecked : styles.checkBox}>
                                                <img src={isExtendedFamilyCur ? circleGreen : circleGray} alt="" height={20} width={20} />
                                            </button>
                                            {/* <input type="checkbox" checked={isExtendedFamilyCur} onBlur={(e) => { e.preventDefault(); ChangeInfo() }} onChange={(e) => { setIsExtendFamily(!isExtendedFamilyCur) }} /> */}
                                        </div>
                                    </div>
                                )}
                                {role == 3 && (
                                    <div className={styles.settingOrganization}>
                                        <div className={styles.inputDataExtend}>
                                            <label >Название</label>
                                            <input type="text" placeholder="адрес" onChange={(e) => { setNameOrganization(e.target.value) }} />
                                        </div>
                                        <div className={styles.inputDataExtend}>
                                            <label >Контакты</label>
                                            <input type="text" placeholder="контакты" defaultValue={contactsCur} onChange={(e) => { setContacts(e.target.value) }} />
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