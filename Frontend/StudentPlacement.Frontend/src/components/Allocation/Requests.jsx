import styles from "./Allocation.module.css"
import whitePlus from "../../assets/Allocation/whitePlus.png"

const Requests = ({ Requests, idStudent, AddRequestToUser }) => {


    return (
        <section className={styles.personalRequests}>
            {
                Requests.filter(request => request.countFreeSpace > 0).length == 0 && (
                    <div className={styles.emptyRequests}>
                        <p>Нету свободных заявок</p>
                    </div>
                )
            }
            {Requests.map(request => {
                if (request.countFreeSpace === 0) return null;

                return (
                    <div className={styles.oneRequest} key={request.idOrganization}>
                        <div className={styles.oneRequestInfo}>
                            <p>{request.nameOrganization}</p>
                            <p>{request.specialist}</p>
                            <p>{request.adress}</p>
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

export default Requests;