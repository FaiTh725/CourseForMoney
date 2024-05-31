import { useContext, useEffect, useRef, useState } from "react"
import styles from "./Structure.module.css"
import useUpdateToken from "../../hooks/useUpdateToken";
import useParseToken from "../../hooks/useParseToken";
import useRedirectionRefreshToken from "../../hooks/useRedirectionRefreshToken";
import api from "../../api/helpAxios";
import AuthContext from "../Context/AuthProvider";
import { useNavigate } from "react-router-dom";
import Modal from "../Modal/Modal";
import MiniModal from "../Modal/MiniModal";

// сделать красивую выборку из специальностей и кафедр
// валидация ввода
// добавить удаление
const Structure = () => {
    const [createElement, setCreateElement] = useState("DepartmentCreate");
    // const [departments, setDepartments] = useState([]);
    const [specialities, setSpecialities] = useState([]);
    const [modalActive, setModalActive] = useState(false);
    const [miniModalActive, setMiniModalActive] = useState(false);

    const modalError = useRef(null);
    const miniMidalError = useRef(null);

    const { auth, setAuth } = useContext(AuthContext);
    const navigate = useNavigate();

    const GetAllSpecializations = async () => {
        try {
            const token = localStorage.getItem("token");

            const response = await api.get("/Structure/GetAllSpecialities",
                {
                    withCredentials: true,
                    headers: {
                        'Authorization': `Bearer ${token == null ? "" : token}`
                    }
                }
            );

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
                return;
            }

            setSpecialities(response.data.data);
            console.log(response.data.data);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetAllSpecializations() },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    // const GetAllDepartment = async () => {
    //     try {
    //         const token = localStorage.getItem("token");

    //         const response = await api.get("/Structure/GetAllDepartments",
    //             {
    //                 withCredentials: true,
    //                 headers: {
    //                     'Authorization': `Bearer ${token == null ? "" : token}`
    //                 }
    //             }
    //         );

    //         if (response.data.statusCode != 0) {
    //             console.log(response.data.description);
    //             return;
    //         }

    //         console.log(response);
    //         setDepartments(response.data.data);
    //     }
    //     catch (error) {
    //         console.log(error);
    //         if (error.request.status == 0) {
    //             await useRedirectionRefreshToken(() => { GetAllDepartment() },
    //                 setAuth,
    //                 navigate,
    //                 useUpdateToken,
    //                 useParseToken);
    //         }
    //     }
    // }

    useEffect(() => {
        //const fatchAllDepartments = async () => { await GetAllDepartment() };
        const fatchAllSpecializations = async () => { await GetAllSpecializations() };

        //fatchAllDepartments();
        fatchAllSpecializations();
    }, []);

    return (
        <main className={styles.main}>
            <header className={styles.header}>
                <h1>Создание структуры университета</h1>
            </header>
            <section className={styles.optionsAdd}>
                <div>
                    <p>Создать</p>
                </div>
                <Modal active={modalActive} setActive={setModalActive}>
                    <p ref={modalError}></p>
                </Modal>
                <MiniModal active={miniModalActive} setActive={setMiniModalActive}>
                    <p ref={miniMidalError}></p>
                </MiniModal>
                <ul>
                    <li className={createElement == "DepartmentCreate" ? styles.tabChacked : styles.settionOption} onClick={() => { setCreateElement("DepartmentCreate") }}>Кафедра</li>
                    <li className={createElement == "SpecializationCreate" ? styles.tabChacked : styles.settionOption} onClick={() => { setCreateElement("SpecializationCreate") }}>Специльность</li>
                    <li className={createElement == "GroupCreate" ? styles.tabChacked : styles.settionOption} onClick={() => { setCreateElement("GroupCreate") }}>Группа</li>
                </ul>
            </section>
            <section className={styles.addElemnt}>
                {createElement == "DepartmentCreate" && (<DepartmentCreate navigate={navigate} setAuth={setAuth} setMiniModalActive={setMiniModalActive} setModalActive={setModalActive} modalMessage={modalError} miniModalMessage={miniMidalError} departments={specialities} />)}
                {createElement == "SpecializationCreate" && (<SpecializationCreate departments={specialities} navigate={navigate} setAuth={setAuth} setMiniModalActive={setMiniModalActive} setModalActive={setModalActive} modalMessage={modalError} miniModalMessage={miniMidalError} />)}
                {createElement == "GroupCreate" && (<GroupCreate specialities={specialities} navigate={navigate} setAuth={setAuth} setMiniModalActive={setMiniModalActive} setModalActive={setModalActive} modalMessage={modalError} miniModalMessage={miniMidalError} />)}
            </section>
        </main>
    )
}

const GroupCreate = ({ specialities, navigate, setAuth,
    setModalActive, setMiniModalActive, modalMessage, miniModalMessage }) => {
    const [selectedSpecialization, setSelectedSpecialization] = useState([]);
    const [specialization, setSpecialization] = useState(null);

    const [number, setNumber] = useState("");

    const numberError = useRef(null);

    useEffect(() => {
        if (specialities.length > 0) {
            setSelectedSpecialization(specialities[0].departmentSpeciality);
            if (specialities[0].departmentSpeciality.length > 0) {
                setSpecialization(specialities[0].departmentSpeciality[0].id);
            }
        }
    }, []);

    // useEffect(() => {
    //     console.log(selectedSpecialization);
    //     if (selectedSpecialization.departmentSpeciality.length>0) {
    //         console.log("Условие выполнилось");
    //         setSpecialization(selectedSpecialization.departmentSpeciality[0].id);
    //     }
    // }, [selectedSpecialization]);

    const CreateGroup = async (e) => {
        var flag = false;

        if (number == "") {
            numberError.current.textContent = "Это поле обязательное";
            flag = true;
        }

        if (flag) return;

        if (specialities.length == 0 || selectedSpecialization.length == 0) {
            setModalActive(true);
            modalMessage.current.textContent = "Добавьте кафедру и специальность для добавлении группы";
            // alert("Добавьте кафедру и специальность для добавлении группы");
            return;
        }

        console.log(specialization);

        try {
            const token = localStorage.getItem("token");

            const response = await api.post("/Structure/CreateGroup",
                {
                    number: number,
                    idSpeciality: specialization
                },
                {
                    withCredentials: true,
                    headers:
                    {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token ?? ""}`
                    }
                }
            );

            if (response.data.statusCode == 9) {
                setModalActive(true);
                modalMessage.current.textContent = "Группа уже существует";
                //alert("Группа уже существует");
                return;
            }
            if (response.data.statusCode == 8) {
                setModalActive(true);
                modalMessage.current.textContent = "Неверная выбранная группа";
                //alert("Неверная выбранная группа");
                return;
            }
            if (response.data.statusCode != 0) {
                setModalActive(true);
                modalMessage.current.textContent = response.data.description;
                //alert("Ошибка серве");
                return;
            }

            console.log(response);
            miniModalMessage.current.textContent = "Создали специальность";
            setMiniModalActive(true);
            setNumber("");
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { CreateGroup(e) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    return (
        <section className={styles.variantAdd}>
            <div className={styles.inputData}>
                <label>Номер</label>
                <input value={number} type="text" onChange={(e) => setNumber(e.target.value)} />
                <label ref={numberError}></label>
            </div>
            <div className={styles.inputData}>
                <label>Кафедра</label>
                <select defaultValue={selectedSpecialization} onChange={(e) => { setSelectedSpecialization(JSON.parse(e.target.value)) }}>
                    {
                        specialities.map(speciality => (
                            <option key={speciality.id} value={JSON.stringify(speciality.departmentSpeciality)}>{speciality.name}</option>
                        ))
                    }
                </select>
            </div>
            <div className={styles.inputData}>
                <label>Специльность</label>
                <select defaultValue={specialization} onChange={(e) => { setSpecialization(e.target.value) }}>
                    {
                        selectedSpecialization.map(specialization => (
                            <option key={specialization.id} value={specialization.id}>{specialization.name}</option>
                        ))
                    }
                </select>
            </div>
            <div className={styles.createContainerBtn}>
                <button onClick={(e) => { CreateGroup(e) }}>Добавить</button>
            </div>
        </section>
    )
}

const SpecializationCreate = ({ departments, navigate, setAuth,
    setModalActive, setMiniModalActive, modalMessage, miniModalMessage }) => {
    const [selectedDepartment, setSelectedDepartment] = useState(null);
    const [name, setName] = useState("");
    const [shortName, setShortName] = useState("");
    const [code, setCode] = useState("");

    const nameError = useRef(null);
    const codeError = useRef(null);
    const shortNameError = useRef(null);

    useEffect(() => {
        nameError.current.textContent = "";
        codeError.current.textContent = "";
        shortNameError.current.textContent = "";
    }, [selectedDepartment, name, shortName, code]);

    useEffect(() => {
        if (departments.length > 0) {
            console.log(departments[0]);
            setSelectedDepartment(departments[0].id);
        }
    }, []);

    useEffect(() => {
        console.log(departments.find(department => department.id == selectedDepartment));
    }, [selectedDepartment]);

    const CreateSpecialization = async (e) => {
        if (selectedDepartment == null) {
            modalMessage.current.textContent = "Добавить фафедру для группы";
            setModalActive(true);
            //alert("Для начала добавьте кафедру для группы");
            return;
        }

        var flag = false;
        if (name == "") {
            nameError.current.textContent = "Это поле обязательное";
            flag = true;
        }
        if (code == "") {
            codeError.current.textContent = "Это поле обязательное";
            flag = true;
        }
        if (shortName == "") {
            shortNameError.current.textContent = "Это поле обязательное";
            flag = true;
        }

        if (flag) {
            return;
        }


        try {
            const token = localStorage.getItem("token");

            const response = await api.post("/Structure/CreateSpeciality",
                {
                    name: name,
                    shortName: shortName,
                    code: code,
                    idDepartment: selectedDepartment
                },
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token ?? ""}`
                    }
                }
            );

            if (response.data.statusCode == 6) {
                setModalActive(true);
                modalMessage.current.textContent = "Ошибка при выборекафедры";
                // alert("Ошибка при выборекафедры");
                return;
            }
            if (response.data.statusCode == 7) {
                setModalActive(true);
                modalMessage.current.textContent = "Специальность уже существует";
                // alert("Специальность уже существует");
                return;
            }
            if (response.data.statusCode != 0) {
                setModalActive(true);
                modalMessage.current.textContent = response.data.description;
                //alert(response.data.description);
                return;
            }

            var updateDepartment = departments.find(department => department.id == selectedDepartment);
            console.log(updateDepartment);
            updateDepartment.departmentSpeciality.push({ id: response.data.data.id, name: response.data.data.name });
            console.log(updateDepartment);

            console.log(response);
            miniModalMessage.current.textContent = "Создали специальность";
            //departments.push({});
            setMiniModalActive(true);
            setCode("");
            setName("");
            setShortName("");
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { CreateSpecialization(e) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    };

    return (
        <section className={styles.variantAdd}>
            <div className={styles.inputData}>
                <label>Название</label>
                <input value={name} type="text" onChange={(e) => { setName(e.target.value) }} />
                <label ref={nameError}></label>
            </div>
            <div className={styles.inputData}>
                <label>Короткое имя</label>
                <input value={shortName} type="text" onChange={(e) => { setShortName(e.target.value) }} />
                <label ref={shortNameError}></label>
            </div>
            <div className={styles.inputData}>
                <label>Код</label>
                <input value={code} type="text" onChange={(e) => { setCode(e.target.value) }} />
                <label ref={codeError}></label>
            </div>
            <div className={styles.inputData}>
                <label>Кафедра</label>
                <select defaultValue={selectedDepartment} onChange={(e) => { setSelectedDepartment(e.target.value) }}>
                    {
                        departments.map(department => (
                            <option value={department.id} key={department.key}>{department.name}</option>
                        ))
                    }
                </select>
            </div>
            <div className={styles.createContainerBtn}>
                <button onClick={(e) => { CreateSpecialization(e) }}>Добавить</button>
            </div>
        </section>
    )
}

const DepartmentCreate = ({ navigate, setAuth, departments,
    setModalActive, setMiniModalActive, modalMessage, miniModalMessage }) => {

    const [name, setName] = useState("");
    const [shortName, setShortName] = useState("");
    const [headOfDepartment, setHeadOfdepartment] = useState("");

    const nameError = useRef(null);
    const shortNameError = useRef(null);
    const headOfDepartmentError = useRef(null);

    useEffect(() => {
        nameError.current.textContent = "";
        shortNameError.current.textContent = "";
        headOfDepartmentError.current.textContent = "";
    }, [name, shortName, headOfDepartment]);

    const CreateDepartment = async (e) => {
        e.preventDefault();

        var flag = false;

        if (name == "") {
            nameError.current.textContent = "Это поле обязательное";
            flag = true;
        }
        if (shortName == "") {
            shortNameError.current.textContent = "Это поле обязательное";
            flag = true;
        }
        if (headOfDepartment == "") {
            headOfDepartmentError.current.textContent = "Это поле обязательное";
            flag = true;
        }

        if (flag) return;


        try {
            const token = localStorage.getItem("token");

            const response = await api.post("/Structure/CreateDepartment",
                {
                    name: name,
                    shortName: shortName,
                    headOfDepartment: headOfDepartment
                },
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token ?? ""}`
                    }
                }
            );

            if (response.data.statusCode != 0) {
                setModalActive(true);
                modalMessage.current.textContent = response.data.description;
                // alert(response.data.description);
                return;
            }
            const createdDepartmentId = response.data.data.id;
            const createdDepartmentName = response.data.data.name;

            departments.push({ id: createdDepartmentId, name: createdDepartmentName, departmentSpeciality: [] });

            console.log(response);
            setMiniModalActive(true);
            miniModalMessage.current.textContent = "Создали кафедру";
            setName("");
            setShortName("");
            setHeadOfdepartment("");
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { CreateDepartment(e) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    return (
        <section className={styles.variantAdd}>
            <div className={styles.inputData}>
                <label>Название</label>
                <input value={name} type="text" onChange={(e) => { setName(e.target.value) }} />
                <label ref={nameError}></label>
            </div>
            <div className={styles.inputData}>
                <label>Короткое имя</label>
                <input value={shortName} type="text" onChange={(e) => { setShortName(e.target.value) }} />
                <label ref={shortNameError}></label>
            </div>
            <div className={styles.inputData}>
                <label>Глава кафедры</label>
                <input value={headOfDepartment} type="text" onChange={(e) => { setHeadOfdepartment(e.target.value) }} />
                <label ref={headOfDepartmentError}></label>
            </div>
            <div className={styles.createContainerBtn}>
                <button onClick={(e) => { CreateDepartment(e) }}>Добавить</button>
            </div>
        </section>
    )
}

export default Structure;