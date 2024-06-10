import { useState, useRef, useEffect } from "react"
import styles from "./Profile.module.css"
import useDownloadFileWithUrl from "../../hooks/useDownloadFileWithUrl";
import MiniModal from "../Modal/MiniModal";

import deleteCross from "../../assets/Account/delete_cross.png"
import plus from "../../assets/Account/plus.png"

const RequestOrganization = ({ type, idOrganization,
    DeleteAllocationRequest, ChangeRequest,
    AddAllocationRequest, request }) => {

    const [idOrganizationCur, setIdOrganization] = useState(idOrganization);
    const [idRequest, setIdRequest] = useState(null);
    const [adressRequest, setAdressRequest] = useState("");
    const [specialist, setSpecialist] = useState("");
    const [countFreePlace, setCountFreePlace] = useState("");
    const [urlOrderFile, setUrlOrderFile] = useState("");
    const [orderFile, setOrderFile] = useState(null);

    const inputFile = useRef(null);

    useEffect(() => {
        if (request != null) {
            setIdRequest(request.idRequest);
            setAdressRequest(request.nameRequest);
            setSpecialist(request.specialist);
            setUrlOrderFile(request.urlOrderFile);
            setCountFreePlace(request.countPlace);
        }
    }, []);

    return (
        <form className={type == "view" ? `${styles.organizationRequest} ${styles.viewRequest}`: styles.organizationRequest}>
            <div className={styles.inputData}>
                <label>Адрес</label>
                <input value={adressRequest} onChange={(e) => { setAdressRequest(e.target.value) }} type="text" placeholder="Адрес" />
            </div>
            <div className={styles.inputData}>
                <label>Специалист</label>
                <input value={specialist} onChange={(e) => { setSpecialist(e.target.value) }} type="text" placeholder="Требуемый специалист" />
            </div>
            <div className={styles.inputData}>
                <label>Количество мест</label>
                <input value={countFreePlace} onChange={(e) => { setCountFreePlace(e.target.value) }} type="text" placeholder="Количество мест" />
            </div>
            <div className={styles.inputData}>
                <label>Приказ(.docx)</label>
                <div className={styles.inputFile}>
                    <label onClick={() => { urlOrderFile == null ? alert("Шляпа с файлом") : useDownloadFileWithUrl(urlOrderFile, "Order.docx") }}>{type == "view" ? "Посмотреть файл" : orderFile == null ? "Загрузите файл" : orderFile.name}</label>
                    {
                        type == "add" && (
                            <div>
                                <input ref={inputFile} onChange={(e) => { setOrderFile(e.target.files[0]) }} type="file" accept="*.doc,.docx,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.documen" placeholder="файл в формате .docx" />
                                <button type="button" onClick={() => { inputFile.current.click() }}>выбрать файл</button>
                            </div>
                        )
                    }

                </div>
            </div>
            {
                type == "add" && (
                    <div className={styles.btnContainer} onClick={(e) => {
                        AddAllocationRequest(e, idOrganization,
                            adressRequest, specialist, countFreePlace, orderFile);
                            setAdressRequest("");
                            setSpecialist("");
                            setCountFreePlace("");
                            setOrderFile(null);
                    }}>
                        <button className={styles.addBtn} type="submit">
                            <p>Добавить запрос</p>
                            <img src={plus} alt="add profile" height={35} />
                        </button>
                    </div>
                )
            }
            {
                type == "view" && (
                    <div className={styles.btnContainerView}>
                        <button onClick={(e) => {ChangeRequest(e, idRequest, adressRequest, specialist, countFreePlace)}} className={styles.saveButton} type="button">Сохранить</button>
                        <button onClick={(e) => { DeleteAllocationRequest(e, idRequest, idOrganizationCur) }} className={styles.deleteBtn} type="submit">
                            <p>Удалить запрос</p>
                            <img src={deleteCross} alt="delete profile" height={35} />
                        </button>
                    </div>
                )
            }
        </form>
    )
}

export default RequestOrganization;