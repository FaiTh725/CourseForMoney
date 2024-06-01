import { useContext, useEffect, useRef, useState } from "react";
import styles from "./Account.module.css"
import api from "../../api/helpAxios";
import useParseToken from "../../hooks/useParseToken";
import useUpdateToken from "../../hooks/useUpdateToken";
import { useNavigate } from "react-router-dom";
import AuthContext from "../Context/AuthProvider";
import useRedirectionRefreshToken from "../../hooks/useRedirectionRefreshToken";
import useImgToString from "../../hooks/useImgToString";
import ReactInputMask from "react-input-mask";
import Modal from "../Modal/Modal";

import defaultUserImage from "../../assets/Account/user.png";
import loginImg from "../../assets/Account/login.png";
import passwordImg from "../../assets/Account/password.png"
import studentNameImg from "../../assets/Account/student_name.png"
import adressStudentImg from "../../assets/Account/adress_student.png"
import averageScoreImg from "../../assets/Account/average_score.png"
import organizationNameImg from "../../assets/Account/organization_name.png"
import contactsImg from "../../assets/Account/contacts.png"
import roleImg from "../../assets/Account/role.png"
import groupImg from "../../assets/Account/group.png"
import emailImg from "../../assets/Account/email.png"
import circleGray from "../../assets/Account/circleGray.png"
import circleGreen from "../../assets/Account/circleGree.png"
import errorImg from '/error.png'

const Account = ({SetTab}) => {
    const [uploadFile, setUploadFile] = useState(null);
    const [userImage, setUserImage] = useState("");
    const imgBtn = useRef(null);
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");
    const [selectedRole, setSelectedRole] = useState(0);
    const navigate = useNavigate();
    const { auth, setAuth } = useContext(AuthContext);
    const errorMessage = useRef(null);
    const [modalActive, setModalActive] = useState(false);
    // student setting
    const [groups, setGroups] = useState([]);
    const [selectedGroup, setSelectedGroup] = useState(0);
    const [fullName, setFullName] = useState("");
    const [averageScore, setAverageScore] = useState(0);
    const [adressStudent, setAdressstudent] = useState("");
    const [isMaried, setIsMaried] = useState(false);
    const [isExtendFamily, SetIsExtendFamily] = useState(false);
    // organization setting
    const [nameOrganization, setNameOrganization] = useState("");
    const [contacts, setContacts] = useState("");
    // allocation request
    const [adressAllocationRequest, setAdressAllocationRequest] = useState("");
    const [countSpace, setCountSpace] = useState(0);

    // errors
    const loginError = useRef(null);
    const passwordError = useRef(null);
    const emailError = useRef(null);
    const averageScoreError = useRef(null);

    useEffect(() => {
        loginError.current.textContent = "";

        if (averageScoreError.current != null) {
            averageScoreError.current.textContent = "";
        }

        passwordError.current.textContent = "";
    }, [login, password, averageScore, email]);


    const CreateUser = async (e) => {
        e.preventDefault();

        if (login.length < 4) {
            loginError.current.textContent = "Логин короче 4 символов";
            return;
        }

        if (password.length < 4) {
            passwordError.current.textContent = "Пароль короче 4 символов";
            return;
        }
        else if (!/['a-zA-Z']/.test(password) || !/['0-9']/.test(password)) {
            passwordError.current.textContent = "Пароль должен содержать 1 цифру и 1 букву";
            return;
        }

        if (!(Number.parseFloat(averageScore) >= 0 && Number.parseFloat(averageScore) <= 10)) {
            averageScoreError.current.textContent = "Средний балл должен быть в промежутке от 0 до 10";
            return;
        }

        if (!email.includes("@")) {
            emailError.current.textContent = "Неверная почта";
            return;
        }

        if (selectedRole == 0 && selectedGroup == null) {
            errorMessage.current.value = "Сейчас невозможно добавить студента";
            return;
        }
        try {
            const token = localStorage.getItem("token");

            const formData = new FormData();
            formData.append("login", login);
            formData.append("password", password);
            formData.append("role", selectedRole);
            formData.append("image", uploadFile);
            formData.append("email", email);
            var data = {
                login: login,
                password: password,
                role: selectedRole,
                image: uploadFile,
                email: email,
                group: selectedGroup,
                fullName: "",
                averageScore: 0,
                adressStudent: "",
                isMarried: false,
                extendedFamily: false,
                nameOrganization: "",
                contacts: "",
            };
            if (selectedRole == 0) {
                data = {
                    ...data,
                    group: selectedGroup,
                    fullName: fullName,
                    averageScore: averageScore,
                    adressStudent: adressStudent,
                    isMarried: isMaried,
                    extendedFamily: isExtendFamily
                }
            }
            if (selectedRole == 3) {
                data = {
                    ...data,
                    nameOrganization: nameOrganization,
                    contacts: contacts,
                }
            }
            console.log(data);

            formData.append("group", data.group);
            formData.append("fullName", data.fullName);
            formData.append("averageScore", data.averageScore);
            formData.append("adressStudent", data.adressStudent);
            formData.append("isMarried", data.isMarried);
            formData.append("extendedFamily", data.extendedFamily);
            formData.append("nameOrganization", data.nameOrganization);
            formData.append("contacts", data.contacts);



            const response = await api.post("/Account/CreateAccount",
                // data,
                formData,
                {
                    withCredentials: true,
                    headers: {
                        // 'Content-Type': 'application/json',
                        'Content-Type': 'multipart/form-data',
                        'Authorization': `Bearer ${token == null ? "" : token}`
                    }
                });


            if (response.data.statusCode != 0) {
                errorMessage.current.textContent = response.data.description;
                setModalActive(true);
                // alert(errorMessage.current.value = response.data.description);
                return;
            }

            console.log(response);
            alert("Создали пользователя");
            SetTab(null);

        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { CreateUser(e) },
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

            setGroups(response.data.data);
            response.data.data.length > 0 ? setSelectedGroup(response.data.data[0].id) : null;
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

    useEffect(() => {
        const fatchAllGroup = async () => {
            await GetAllGroup();
        };

        const setDefaultImage = async () => {
            const fileObject = await fetch(defaultUserImage);
            const blob = await fileObject.blob();

            const stringImg = await useImgToString(blob);

            setUserImage(stringImg)
        }

        setDefaultImage();

        fatchAllGroup();
    }, []);

    return (
        <main className={styles.main}>
            <header className={styles.header}>
                <h1>Управдение пользователями</h1>
            </header>
            <Modal active={modalActive} setActive={setModalActive}>
                <p className={styles.errorModal} ref={errorMessage}></p>
                <img src={errorImg} alt="error" width={800} height={600}/>
            </Modal>
            <form onSubmit={(e) => { CreateUser(e) }} className={styles.createAccount}>
                <div className={styles.imageInput}>
                    <img onClick={() => { imgBtn.current.click(); }} src={uploadFile == null ? defaultUserImage : URL.createObjectURL(uploadFile)} alt="user logo" height={140} width={140} />
                    {/* <input maxLength={17} ref={imgBtn} className={styles.inputFile} onChange={(e) => { HandlerImageChanged(e.target.files[0]) }} type="file" accept=".png" /> */}
                    <input maxLength={17} ref={imgBtn} className={styles.inputFile} onChange={(e) => { setUploadFile(e.target.files[0]) }} type="file" accept=".png" />
                </div>
                <div className={styles.writeData}>
                    <div className={styles.dataInput}>
                        <label >Логин</label>
                        <div>
                            <img src={loginImg} alt="login ico" height={25} />
                            <input maxLength={17} onChange={(e) => { setLogin(e.target.value) }} type="text" placeholder="логин" />
                        </div>
                        <label ref={loginError}></label>
                    </div>
                    <div className={styles.dataInput}>
                        <label>Пароль</label>
                        <div>
                            <img src={passwordImg} alt="password" height={25} />
                            <input maxLength={17} onChange={(e) => { setPassword(e.target.value) }} type="text" placeholder="пароль" />
                        </div>
                        <label ref={passwordError}></label>
                    </div>
                    <div className={styles.dataInput}>
                        <label>Почта</label>
                        <div>
                            <img src={emailImg} alt="email" height={25} />
                            <input className={styles.inputEmail} maxLength={30} onChange={(e) => { setEmail(e.target.value) }} type="text" placeholder="пароль" />
                        </div>
                        <label ref={emailError}></label>
                    </div>
                    <div className={styles.options}>
                        <div className={styles.roleOption}>
                            <img src={roleImg} alt="" height={25} />
                            <select maxLength={17} onChange={(e) => { setSelectedRole(e.target.value) }}>
                                <option className={styles.selectOption} value={0}>Студент</option>
                                <option className={styles.selectOption} value={1}>Глава кафедры</option>
                                <option className={styles.selectOption} value={2}>Админ</option>
                                <option className={styles.selectOption} value={3}>Организации</option>
                            </select>
                        </div>
                        <div>
                            {selectedRole == 0 && (
                                <div className={styles.studentGroups}>
                                    <div className={styles.groupOption}>
                                        <img src={groupImg} alt="" height={25} />
                                        <select onChange={(e) => setSelectedGroup(e.target.value)}>
                                            {
                                                groups.map(group => (
                                                    <option className={styles.selectOption} key={group.id} value={group.id}>{group.name}</option>
                                                ))
                                            }
                                        </select>
                                    </div>
                                    <div className={styles.dataInput}>
                                        <label>Полное имя</label>
                                        <div>
                                            <img src={studentNameImg} alt="name student" height={25} />
                                            <input maxLength={17} onChange={(e) => { setFullName(e.target.value) }} type="text" placeholder="полное имя" />
                                        </div>
                                    </div>
                                    <div className={styles.dataInput}>
                                        <label>Средний бал</label>
                                        <div>
                                            <img src={averageScoreImg} alt="averga score" height={25} />
                                            <input maxLength={17} onChange={(e) => { setAverageScore(e.target.value) }} type="text" placeholder="средний балл" />
                                        </div>
                                        <label ref={averageScoreError}></label>
                                    </div>
                                    <div className={styles.dataInput}>
                                        <label>Адрес</label>
                                        <div>
                                            <img src={adressStudentImg} alt="adress student" height={25} />
                                            <input maxLength={17} onChange={(e) => { setAdressstudent(e.target.value) }} type="text" placeholder="адрес" />
                                        </div>
                                    </div>
                                    <div className={styles.dataInput}>
                                        <label>Женат</label>
                                        <button onClick={() => { setIsMaried(!isMaried) }} type="button" className={isMaried == true ? styles.checkBoxChecked : styles.checkBox}>
                                            <img src={isMaried ? circleGreen : circleGray} alt="" height={20} width={20} />
                                        </button>
                                    </div>
                                    <div className={styles.dataInput}>
                                        <label>Многодетная семья</label>
                                        <button onClick={() => { SetIsExtendFamily(!isExtendFamily) }} type="button" className={isExtendFamily == true ? styles.checkBoxChecked : styles.checkBox}>
                                            <img src={isExtendFamily ? circleGreen : circleGray} alt="" height={20} width={20} />
                                        </button>
                                    </div>
                                </div>

                            )}
                            {selectedRole == 3 && (
                                <div className={styles.organizationSetting}>
                                    <div className={styles.dataInput}>
                                        <label>Название</label>
                                        <div>
                                            <img src={organizationNameImg} alt="name organization" height={25} />
                                            <input maxLength={17} onChange={(e) => { setNameOrganization(e.target.value) }} type="text" placeholder="Название" />
                                        </div>
                                    </div>
                                    <div className={styles.dataInput}>
                                        <label>Контакты</label>
                                        <div>
                                            <img src={contactsImg} alt="organization contacts" height={25} />
                                            <ReactInputMask onChange={(e) => { setContacts(e.target.value) }} mask="+375 (99) 999-99-99" placeholder="+375 (99) 999-99-99" />
                                        </div>
                                    </div>
                                </div>
                            )}
                        </div>
                    </div>
                    <div className={styles.btnContainer}>
                        <button className={styles.defaultBtn} type="submit">Создать</button>
                    </div>
                </div>

            </form>
        </main>
    )
}

export default Account;