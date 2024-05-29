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
import circleGray from "../../assets/Account/circleGray.png"
import circleGreen from "../../assets/Account/circleGree.png"
import triangleDown from "../../assets/Allocation/downArrow.png"

// по клике на заявку показать всех студентов в таблице
// фильтры по студентам и поиск
const Allocation = () => {
    const [allDepartments, setAllDepartments] = useState([]);
    const [specialities, setSpecialities] = useState([]);
    const [groups, setGroups] = useState([]);
    const [selectedGroup, setSelectedGroup] = useState(null);

    const [allAllocationRequest, setAllAllocationRequest] = useState([]);
    const [allStudent, setAllStudent] = useState([]);

    const [modalStudentIsOpen, setModalStudentIsOpen] = useState({});

    // filters 
    const [allocationFilter, setAllocationfilter] = useState(true);
    const [notAllocationFilter, setNotAllocationFilter] = useState(true);
    const [nameFilter, setNameFilter] = useState(null);
    const [averageFilter, setAverageFilter] = useState(null);

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
            const newReq = allAllocationRequest.map(req => {
                if (req.idRequest == idRequest) {
                    console.log("прибавили 1");
                    req.countFreeSpace++;
                }

                return req;
            });

            setAllAllocationRequest(newReq);

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
            setAllStudent(newStudents);

            const newRequests = allAllocationRequest.map(request => {
                if (request.idRequest == idRequest) {
                    console.log("Отняли 1");
                    request.countFreeSpace--;
                }

                return request;
            });

            setAllAllocationRequest(newRequests);
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

    const DownloadReportAllocation = async (e) => {
        e.preventDefault();

        try {
            const token = localStorage.getItem("token")

            const response = await api.get("/Report/GetReport/",
                {
                    params: {
                        idGroup: selectedGroup
                    },
                    withCredentials: true,
                    headers: {
                        'Authorization': `Bearer ${token == null ? "" : token}`,
                    },
                    responseType: 'blob'
                }
            );

            const href = URL.createObjectURL(response.data);

            const link = document.createElement('a');
            link.href = href;
            link.setAttribute('download', 'AllocationReport.docx');
            document.body.appendChild(link);
            link.click();

            document.body.removeChild(link);
            URL.revokeObjectURL(href);

            console.log(response);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { DownloadReportAllocation() },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    useEffect(() => {
        console.log(allAllocationRequest);
    }, [allAllocationRequest]);

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
                        <div className={styles.cardRequestMain}>
                            <p>Название организации</p>
                            <p>Контакты</p>
                            <p>Количество свободных мест</p>
                        </div>
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
                selectedGroup != null && (
                    <section className={styles.reportContainer}>
                        <div className={styles.filters}>
                            <section className={styles.filterRequest}>
                                <p>Заявка</p>
                                <div>
                                    <p>Распределен</p>
                                    <button type="button" onClick={(e) => {setAllocationfilter(!allocationFilter)}} className={allocationFilter == true ? styles.checkBoxChecked : styles.checkBox}>
                                        <img src={allocationFilter ? circleGreen : circleGray} alt="" height={20} width={20} />
                                    </button>
                                </div>
                                <div>
                                    <p>Не распределен</p>
                                    <button type="button" onClick={(e) => {setNotAllocationFilter(!notAllocationFilter)}} className={notAllocationFilter == true ? styles.checkBoxChecked : styles.checkBox}>
                                        <img src={notAllocationFilter ? circleGreen : circleGray} alt="" height={20} width={20} />
                                    </button>
                                </div>
                            </section>
                        </div>
                        <div className={styles.reportButtonContainer}>
                            <button onClick={(e) => { DownloadReportAllocation(e) }} className={styles.getReportBtn}>Ведомость</button>
                        </div>
                    </section>
                )
            }
            {
                allStudent.length > 0 && (
                    <table className={styles.allStudent}>
                        <thead className={styles.tableHead}>
                            <tr>
                                <th onClick={() => {setNameFilter(nameFilter == null ? true : !nameFilter)}} className={`${styles.firstColumn}`}>
                                    <div className={styles.tableHeadCell}>
                                        <p>Полное имя</p>
                                        <img className={nameFilter == null ? styles.filterTrianleHide: nameFilter == true ? styles.filterTrianleUp: styles.filterTrianleDown} src={triangleDown} alt="arrow" height={15} width={15}/>
                                    </div>
                                </th>
                                <th onClick={() => {setAverageFilter(averageFilter == null ? true : !averageFilter)}}>
                                    <div className={styles.tableHeadCell}>
                                        <p>Средний балл</p>
                                        <img className={averageFilter == null ? styles.filterTrianleHide: averageFilter == true ? styles.filterTrianleUp: styles.filterTrianleDown} src={triangleDown} alt="arrow" height={15} width={15}/> 
                                    </div>   
                                </th>
                                <th>Статус</th>
                                <th className={styles.lastColumn}>Заявка</th>
                            </tr>
                        </thead>
                        <tbody className={styles.tableBody}>
                            {
                                allStudent.map(student => (
                                    <CardStudentWithRequest key={student.idStudent} idStudent={student.idStudent}
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
            {Requests.map(request => {
                if (request.countFreePlace === 0) return null;

                return (
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
                );
            })}

        </section >
    )
}

const CardStudentWithRequest = ({ idStudent, fullName, averageScore,
    currentRequest, status, request,
    setModalStudentIsOpen, modalStudentIsOpen, DeleteRequestUser,
    AddRequestToUser }) => {

    const [fullOpenRequest, setFullOpenRequest] = useState(false);

    return (
        <tr key={idStudent}>
            <td>{fullName}</td>
            <td>{averageScore}</td>
            <td>{status == 0 ? "Распределен" : "Не распределен"}</td>
            <td className={styles.sectionOptionRequest}>{/*если есть заявка просто ее показать*/}
                {
                    currentRequest.idRequest == null && (
                        <div className={styles.pickRequest}>
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
                                <div className={styles.requestUser} onClick={() => { setFullOpenRequest(!fullOpenRequest) }}>
                                    <p>{currentRequest.nameOrganization}</p>
                                    <p>{currentRequest.adressRequest}</p>
                                    <p>{currentRequest.contacts}</p>
                                </div>
                            )}
                            {fullOpenRequest == true && (
                                <div className={styles.requestUser} onClick={() => { setFullOpenRequest(!fullOpenRequest) }}>
                                    <div>
                                        <label>Организация</label>
                                        <p>{currentRequest.nameOrganization}</p>
                                    </div>
                                    <div>
                                        <label>Адрес заявки</label>
                                        <p>{currentRequest.adressRequest}</p>
                                    </div>
                                    <div>
                                        <label>Контакты</label>
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
    const [countFreePlaceCur, setCountFreePlace] = useState(countFreePlace);

    useEffect(() => {
        setId(id);
        setNameOrganization(nameOrganization);
        setContacts(contacts);
        setCountPlace(countPlace);
        setCountFreePlace(countFreePlace);
    }, [id, nameOrganization, contacts, countPlace, countFreePlace]);

    return (
        <div className={styles.cardRequestMain}>
            <p>{nameOrganizationCur}</p>
            <p>{contactsCur}</p>
            <p>{countFreePlaceCur}</p>
        </div>
    )
}

export default Allocation;