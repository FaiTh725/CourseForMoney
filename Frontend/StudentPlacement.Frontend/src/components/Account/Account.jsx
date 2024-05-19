import { useContext, useEffect, useRef, useState } from "react";
import styles from "./Account.module.css"
import api from "../../api/helpAxios";
import useParseToken from "../../hooks/useParseToken";
import useUpdateToken from "../../hooks/useUpdateToken";
import { useNavigate } from "react-router-dom";
import AuthContext from "../Context/AuthProvider";
import useRedirectionRefreshToken from "../../hooks/useRedirectionRefreshToken";

// валидация логина и пароля
// валидацию данных
// загрузка фото
// маску для телефона для контактов организации
// соединить создание и управление пользователями в одно 
const Account = () => {
    const [uploadFile, setUploadFile] = useState(null);
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [selectedRole, setSelectedRole] = useState(0);
    const navigate = useNavigate();
    const { auth, setAuth } = useContext(AuthContext);
    const errorMessage = useRef(null);
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

    useEffect(() => {

    }, [login, password, uploadFile,
        selectedRole, selectedGroup, fullName,
        adressAllocationRequest, adressStudent,
        averageScore, isMaried, isExtendFamily,
        nameOrganization, contacts, countSpace]);

    const CreateUser = async (e) => {
        //console.log(selectedGroup);
        e.preventDefault();

        if (selectedRole == 0 && selectedGroup == null) {
            errorMessage.current.value = "Сейчас невозможно добавить студента";
            return;
        }
        try {
            const token = localStorage.getItem("token");
            var data = {
                login: login,
                password: password,
                role: selectedRole,
                group: selectedGroup,
                fullName: "",
                averageScore: 0,
                adressStudent: "",
                isMarried: false,
                extendedFamily: false,
                nameOrganization: "",
                contacts: "",
                // adressAllocationRequest: "",
                // countPlace: 0
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
                    // adressAllocationRequest: adressAllocationRequest,
                    // countPlace: countSpace
                }
            }

            const response = await api.post("/Account/CreateAccount",
                data,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token == null ? "" : token}`
                    }
                });


            if (response.data.statusCode != 0) {
                errorMessage.current.value = response.data.description;
                return;
            }
            console.log(response);

            if (response.data.statusCode != 0) {
                errorMessage.current.textCpntent = response.data.description;
                return;
            }


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

        fatchAllGroup();
    }, []);

    return (
        <main className={styles.main}>
            <header className={styles.header}>
                <img src="" alt="icon" />
                <h1>Управдение пользователями</h1>
            </header>
            <form onSubmit={(e) => { CreateUser(e) }} className={styles.creatAccount}>
                <div className={styles.dataInput}>
                    <img src="" alt="user logo" />
                    <input className={styles.inputFile} onChange={(e) => { setUploadFile(e.target.files[0]) }} type="file" accept=".png" />
                </div>
                <div className={styles.dataInput}>
                    <label >Логин</label>
                    <input onChange={(e) => { setLogin(e.target.value) }} type="text" placeholder="логин" />
                </div>
                <div className={styles.dataInput}>
                    <label>Пароль</label>
                    <input onChange={(e) => { setPassword(e.target.value) }} type="text" placeholder="пароль" />
                </div>
                <div className={styles.options}>
                    <div className={styles.roleOption}>
                        <select onChange={(e) => { setSelectedRole(e.target.value) }}>
                            <option value={0}>Студент</option>
                            <option value={1}>Глава кафедры</option>
                            <option value={2}>Админ</option>
                            <option value={3}>организации</option>
                        </select>
                    </div>
                    <div>
                        {selectedRole == 0 && (
                            <div className={styles.studentGroups}>
                                <select onChange={(e) => setSelectedGroup(e.target.value)}>
                                    {
                                        groups.map(group => (
                                            <option key={group.id} value={group.id}>{group.name}</option>
                                        ))
                                    }
                                </select>
                                <div className={styles.dataInput}>
                                    <label>Полное имя</label>
                                    <input onChange={(e) => { setFullName(e.target.value) }} type="text" placeholder="полное имя" />
                                </div>
                                <div className={styles.dataInput}>
                                    <label>Средний бал</label>
                                    <input onChange={(e) => { setAverageScore(e.target.value) }} type="text" placeholder="средний балл" />
                                </div>
                                <div className={styles.dataInput}>
                                    <label>Адрес</label>
                                    <input onChange={(e) => { setAdressstudent(e.target.value) }} type="text" placeholder="адрес" />
                                </div>
                                <div className={styles.dataInput}>
                                    <label>Женат</label>
                                    <input onChange={() => { setIsMaried(!isMaried) }} className={styles.checkBox} type="checkbox" checked={isMaried}></input>
                                </div>
                                <div className={styles.dataInput}>
                                    <label>Многодетная семья</label>
                                    <input onChange={() => { SetIsExtendFamily(!isExtendFamily) }} className={styles.checkBox} type="checkbox" checked={isExtendFamily}></input>
                                </div>
                            </div>

                        )}
                        {selectedRole == 3 && (
                            <div className={styles.organizationSetting}>
                                <div className={styles.dataInput}>
                                    <label>Название</label>
                                    <input onChange={(e) => { setNameOrganization(e.target.value) }} type="text" placeholder="Название" />
                                </div>
                                <div className={styles.dataInput}>
                                    <label>Контакты</label>
                                    <input onChange={(e) => { setContacts(e.target.value) }} type="text" placeholder="Контакты" />
                                </div>
                                {/* <div className={styles.requestSetting}>
                                    <p>Заявка на распределение</p>
                                    <div className={styles.dataInput}>
                                        <label>Адрес</label>
                                        <input onChange={(e) => { setAdressAllocationRequest(e.target.value) }} type="text" placeholder="адрес" />
                                    </div>
                                    <div className={styles.dataInput}>
                                        <label>Количество мест</label>
                                        <input onChange={(e) => { setCountSpace(e.target.value) }} type="text" placeholder="количество мест" />
                                    </div>
                                </div> */}
                            </div>
                        )}
                    </div>
                </div>
                <div>
                    <label ref={errorMessage}></label>
                </div>
                <div className={styles.containerBtn}>
                    <button type="submit">создать</button>
                </div>
            </form>
        </main>
    )
}

export default Account;