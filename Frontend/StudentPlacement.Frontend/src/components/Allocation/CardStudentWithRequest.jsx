import { useState } from "react";
import styles from "./Allocation.module.css"

import crossDelete from "../../assets/Account/delete_cross.png"

import Requests from "./Requests"

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
                                    <p>{currentRequest.contacts}</p>
                                </div>
                            )}
                            {fullOpenRequest == true && (
                                <div>
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
                                        <div>
                                            <label>Специалист</label>
                                            <p>{currentRequest.specialist}</p>
                                        </div>
                                    </div>
                                    <div className={styles.orderFileBtnContainer}>
                                        <button type="button" onClick={() => {useDownloadFileWithUrl(currentRequest.urlOrderFile)}}>
                                            Ведомость
                                            </button>
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

export default CardStudentWithRequest;