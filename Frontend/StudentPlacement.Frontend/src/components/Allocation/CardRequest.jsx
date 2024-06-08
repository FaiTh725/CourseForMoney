import { useState, useEffect } from "react";
import api from "../../api/helpAxios";
import Modal from "../Modal/Modal";
import useDownloadFileWithUrl from "../../hooks/useDownloadFileWithUrl";
import styles from "./Allocation.module.css"

import checked from "../../assets/Allocation/checked.png"
import unchecked from "../../assets/Allocation/unchecked.png"

const CardRequest = ({ idOrganization, idRequest, nameOrganization, 
    contacts, specialist, adress, countPlace, countFreePlace, urlOrderFile,
    setAuth, navigate 
}) => {
    const [idOrganizationCur, setIdOrganization] = useState(idOrganization);
    const [idRequestCur, setIdRequest] = useState(idRequest);
    const [nameOrganizationCur, setNameOrganization] = useState(nameOrganization);
    const [contactsCur, setContacts] = useState(contacts);
    const [specialistCur, setSpecialist] = useState(specialist);
    const [adressCur, setAdress] = useState(adress);
    const [countPlaceCur, setCountPlace] = useState(countPlace);
    const [countFreePlaceCur, setCountFreePlace] = useState(countFreePlace);
    const [urlOrderFileCur, setUrlOrderFileCur] = useState(urlOrderFile);

    const [studentsRequest, setStudentRequest] = useState([]);

    const [modaActive, setModalActive] = useState(false);


    useEffect(() => {
        setIdOrganization(idOrganization);
        setIdRequest(idRequest);
        setNameOrganization(nameOrganization);
        setContacts(contacts);
        setSpecialist(specialist)
        setAdress(adress)
        setCountPlace(countPlace);
        setCountFreePlace(countFreePlace);
        setUrlOrderFileCur(urlOrderFile);
    }, [idOrganization, idRequest, nameOrganization, contacts, 
        specialist, adress, countPlace, countFreePlace, urlOrderFile]);

    const GetStudentFromRequest = async (idRequest) => {
        try {
            const token = localStorage.getItem("token");

            const response = await api.get("/Allocation/GetStudentsFromRequest", {
                params: {
                    idRequest: idRequest
                },
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token ?? ""}`
                }
            });

            if (response.data.statusCode != 0) {
                console.log(response.data.description);
                return;
            }

            setStudentRequest(response.data.data);
            console.log(response);
            setModalActive(!modaActive);
        }
        catch (error) {
            console.log(error);
            if (error.request.status == 0) {
                await useRedirectionRefreshToken(() => { GetStudentFromRequest(idRequest) },
                    setAuth,
                    navigate,
                    useUpdateToken,
                    useParseToken);
            }
        }
    }

    return (
        <div className={styles.cardRequestMain} onClick={(e) => { GetStudentFromRequest(idRequest)}}>
            <Modal onClick={(e) => {e.stopPropagation()}} active={modaActive} setActive={setModalActive}>
                <p>Студенты заявки</p>
                <div className={styles.orderFileBtnContainer}>
                    <button type="button" onClick={() => {useDownloadFileWithUrl(urlOrderFileCur, "Order.docx")}}>Ведомость</button>
                </div>
                {
                    studentsRequest.length == 0 && (
                        <p className={styles.emptyRequestMessage}>
                            Сюда еще не определили студентов
                        </p>
                    )
                }
                {
                    studentsRequest.length > 0 && (
                        studentsRequest.map(student => (
                            <div className={styles.infoStudentInRequest} key={student.id}>
                                <p>{student.fullName}</p>
                                <p>{student.averageScore}</p>
                                <p>{student.adress}</p>
                                <p><img src={student.isMarried ? checked : unchecked} alt="" width={30} height={30}/></p>
                                <p><img src={student.extendedFamily ? checked : unchecked} alt="" width={30} height={30}/></p>
                            </div>
                        ))
                    )
                }
            </Modal>
            <p>{nameOrganizationCur}</p>
            <p>{contactsCur}</p>
            <p>{specialistCur}</p>
            <p>{adressCur}</p>
            <p>{countFreePlaceCur}</p>
        </div>
    )
};

export default CardRequest;

