import styles from "./Allocation.module.css"
import useParseToken from "../../hooks/useParseToken"
import useUpdateToken from "../../hooks/useUpdateToken"
import api from "../../api/helpAxios"
import useRedirectionRefreshToken from "../../hooks/useRedirectionRefreshToken"
import { useNavigate } from "react-router-dom"
import { useContext, useEffect, useState } from "react"
import AuthContext from "../Context/AuthProvider"

import whitePlus from "../../assets/Allocation/whitePlus.png"
import crossDelete from "../../assets/Account/delete_cross.png"

// по клике на заявку показать всех студентов в модальном окне
// по клике на заявку у студента показывать доп инфу
// порабоать с колличеством свободных мест
const Allocation = () => {
    const [allDepartments, setAllDepartments] = useState([]);
    const [specialities, setSpecialities] = useState([]);
    const [groups, setGroups] = useState([]);
    const [selectedGroup, setSelectedGroup] = useState();

    const [allAllocationRequest, setAllAllocationRequest] = useState([]);
    const [allStudent, setAllStudent] = useState([]);

    const [modalStudentIsOpen, setModalStudentIsOpen] = useState({});

    const navigate = useNavigate();
    const { auth, setAuth } = useContext(AuthContext);

    const GetAllAllocationRequest = async () => {
        try {
            const token = localStorage.getItem("token");

            const response = await api.get("/Allocation/GetAllRequests", {
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

            setAllAllocationRequest(response.data.data);
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

    const GetAllDepartmentsAndGroups = async () => {
        try {
            const token = localStorage.getItem("token");

            const response = await api.get("/Allocation/GetDepartmentsAndGroups", {
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
            setAllDepartments(response.data.data);

        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetAllDepartmentsAndGroups() },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    };

    const GetAllStudent = async (e, idGroup) => {
        e.preventDefault();

        try {
            const token = localStorage.getItem("token");

            const response = await api.get("/Allocation/GetUsers",
                {
                    params: {
                        idGroup: idGroup
                    },
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

            setAllStudent(response.data.data);
            setModalStudentIsOpen(response.data.data.map(pair => { pair.idStudent, false }));
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetAllStudent(e, idGroup) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    const DeleteRequestUser = async (e, idStudent, idRequest) => {
        e.preventDefault();

        try {
            const token = localStorage.getItem("token");

            const response = await api.patch("/Allocation/DeleteAllocationRequest", {
                idRequest: idRequest,
                idStudent: idStudent
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
            const newReq = allAllocationRequest.filter(req => {
                if (req.idRequest == idRequest) {
                    req.countFreeSpace++;
                    return req;
                }
                else {
                    return req;
                }
            });

            const newStudents = allStudent.map(student => {
                if (student.idStudent == idStudent) {
                    student.status = 1;
                    student.request.idRequest = null;
                    student.request.idOrganization = null;
                    student.request.nameOrganization = null;
                    student.request.contacts = null;
                    student.request.adressRequest = null;
                }
                return student;
            });

            setAllStudent(newStudents);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { DeleteRequestUser(e, idRequest, idStudent) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    const AddRequestToUser = async (e, idRequest, idStudent) => {
        e.preventDefault();

        try {
            const token = localStorage.getItem("token");

            const response = await api.patch("/Allocation/AddAllocationRequest", {
                idRequest: idRequest,
                idStudent: idStudent
            },
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token == null ? "" : token}`
                    }
                });

            if (response.data.statusCode != 0) {
                console.log(response.data.descriptionc);
                return;
            }

            const newStudents = allStudent.map(student => {
                if (student.idStudent == idStudent) {
                    student.status = 0;
                    student.request.idRequest = response.data.data.allocationRequest.idRequest;
                    student.request.idOrganization = response.data.data.allocationRequest.idOrganization;
                    student.request.nameOrganization = response.data.data.allocationRequest.nameOrganization;
                    student.request.contacts = response.data.data.allocationRequest.contacts;
                    student.request.adressRequest = response.data.data.allocationRequest.adressRequest;

                }
                return student;
            });

            console.log(response);
            console.log(newStudents);
            setAllStudent(newStudents);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { AddRequestToUser(e, idRequest, idStudent) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    useEffect(() => {
        const fatchAllDepartmantsAndGroups = async () => { await GetAllDepartmentsAndGroups() };
        const fatchAllAllocationRequest = async () => { await GetAllAllocationRequest() };

        fatchAllDepartmantsAndGroups();
        fatchAllAllocationRequest();
    }, []);

    return (
        <main className={styles.main}>
            <section className={styles.settings}>
                <nav className={styles.navigate}>
                    {
                        allDepartments.length > 0 && (
                            <div>
                                <h3>Кафедры</h3>
                                <section className={styles.navSection}>
                                    {allDepartments.map(department => (
                                        <button className={specialities == department.specializations ? styles.btnNavChecked : styles.btnNav} onClick={(e) => { setGroups([]); setSpecialities(department.specializations) }} key={department.idDepartment}>{department.nameDepartment}</button>
                                    ))}
                                </section>
                            </div>
                        )
                    }

                    {
                        specialities.length > 0 && (
                            <div>
                                <h3>Специальности</h3>
                                <section className={styles.navSection}>
                                    {specialities.map(speciality => (
                                        <button className={groups == speciality.groups ? styles.btnNavChecked : styles.btnNav} onClick={(e) => { setGroups(speciality.groups) }} key={speciality.idSpecialization}>{speciality.shortName}</button>
                                    ))}
                                </section>
                            </div>
                        )
                    }

                    {
                        groups.length > 0 && (
                            <div>
                                <h3>Группы</h3>
                                <section className={styles.navSection}>
                                    {groups.map(group => (
                                        <button className={selectedGroup == group.idGroup ? styles.btnNavChecked : styles.btnNav} onClick={(e) => { setSelectedGroup(group.idGroup); GetAllStudent(e, group.idGroup) }} key={group.idGroup}>{group.nameGroup}</button>
                                    ))}
                                </section>
                            </div>
                        )
                    }

                </nav>
                <div className={styles.requestsContainer}>
                    <h3>Заявки</h3>
                    <div className={styles.requests}>
                        <CardRequest key={-1} nameOrganization={"Название организации"}
                            contacts={"Контакты"}
                            countPlace={0} />
                        {
                            allAllocationRequest.map(request => (
                                <CardRequest key={request.idOrganization} id={request.idOrganization}
                                    nameOrganization={request.nameOrganization}
                                    contacts={request.contacts}
                                    countPlace={request.countSpace}
                                    countFreePlace={request.countFreeSpace} />
                            ))
                        }
                    </div>
                </div>
            </section>
            {
                allStudent.length > 0 && (
                    <table className={styles.allStudent}>
                        <thead className={styles.tableHead}>
                            <tr>
                                <th className={styles.firstColumn}>Полное имя</th>
                                <th>Средний балл</th>
                                <th>Статус</th>
                                <th className={styles.lastColumn}>Заявка</th>
                            </tr>
                        </thead>
                        <tbody className={styles.tableBody}>
                            {
                                allStudent.map(student => (
                                    <CardStudentWithRequest key={student.id} idStudent={student.idStudent}
                                        fullName={student.fullName}
                                        averageScore={student.averageScore}
                                        status={student.status}
                                        request={allAllocationRequest}
                                        currentRequest={student.request}
                                        setModalStudentIsOpen={setModalStudentIsOpen}
                                        modalStudentIsOpen={modalStudentIsOpen}
                                        DeleteRequestUser={DeleteRequestUser}
                                        AddRequestToUser={AddRequestToUser} />
                                ))
                            }
                        </tbody>
                    </table>
                )
            }

        </main>
    )
}

const Requests = ({ Requests, idStudent, AddRequestToUser }) => {


    return (
        <section className={styles.personalRequests}>
            {Requests.map(request => (
                <div className={styles.oneRequest} key={request.idOrganization}>
                    <div className={styles.oneRequestInfo}>
                        <p>{request.nameOrganization}</p>
                        <p>{request.contacts}</p>
                    </div>
                    <div className={styles.btnRequestContainer}>
                        <button onClick={(e) => { AddRequestToUser(e, request.idRequest, idStudent) }} type="button">
                            <img src={whitePlus} alt="add" width={30} height={30} />
                        </button>
                    </div>
                </div>
            ))}
        </section>
    )
}

const CardStudentWithRequest = ({ idStudent, fullName, averageScore,
    currentRequest, status, request,
    setModalStudentIsOpen, modalStudentIsOpen, DeleteRequestUser,
    AddRequestToUser }) => {
    // изменить заявку, заявку у студента и все заявки

    const [fullOpenRequest, setFullOpenRequest] = useState(false);

    return (
        <tr key={idStudent}>
            <td>{fullName}</td>
            <td>{averageScore}</td>
            <td>{status == 0 ? "Распределен" : "Не распределен"}</td>
            <td className={styles.sectionOptionRequest}>{/*если есть заявка просто ее показать*/}
                {
                    currentRequest.idRequest == null && (
                        <div>
                            <button onClick={(e) => { setModalStudentIsOpen((prev => ({ ...prev, [idStudent]: !prev[idStudent] }))) }} type="button">
                                выбрать заявку
                            </button>
                            <div className={modalStudentIsOpen[idStudent] ? styles.modalRequestOPen : styles.modalRequestHide}>
                                <Requests AddRequestToUser={AddRequestToUser} Requests={request} idStudent={idStudent} />
                            </div>
                        </div>
                    )
                }
                {
                    currentRequest.idRequest != null && (
                        <div className={styles.cardRequestUser}>
                            {fullOpenRequest == false && (
                                <div className={styles.requestUser} onClick={() => {setFullOpenRequest(!fullOpenRequest)}}>
                                    <p>{currentRequest.nameOrganization}</p>
                                    <p>{currentRequest.adressRequest}</p>
                                    <p>{currentRequest.contacts}</p>
                                </div>
                            )}
                            {fullOpenRequest == true && (
                                <div className={styles.requestUser} onClick={() => {setFullOpenRequest(!fullOpenRequest)}}>
                                    <div>
                                        <label>Организация</label>
                                        <p>{currentRequest.nameOrganization}</p>
                                    </div>
                                    <div>
                                        <label>Контакты</label>
                                        <p>{currentRequest.adressRequest}</p>
                                    </div>
                                    <div>
                                        <label>Адрес заявки</label>
                                        <p>{currentRequest.contacts}</p>
                                    </div>
                                </div>
                            )}
                            <div className={styles.requestUserContainerBtn}>
                                <button onClick={(e) => { DeleteRequestUser(e, idStudent, currentRequest.idRequest) }}>
                                    <img src={crossDelete} alt="delete request" width={35} height={35} />
                                </button>
                            </div>
                        </div>
                    )
                }
            </td>
        </tr>
    )
}

const CardRequest = ({ id, nameOrganization, contacts, countPlace, countFreePlace }) => {
    const [idCur, setId] = useState(id);
    const [nameOrganizationCur, setNameOrganization] = useState(nameOrganization);
    const [contactsCur, setContacts] = useState(contacts);
    const [countPlaceCur, setCountPlace] = useState(countPlace);
    const [countFreePlaceCur, setCountFeePlace] = useState(countFreePlace);

    return (
        <div className={styles.cardRequestMain}>
            <p>{nameOrganizationCur}</p>
            <p>{contactsCur}</p>
            <p>{countFreePlaceCur}</p>
        </div>
    )
}

export default Allocation;