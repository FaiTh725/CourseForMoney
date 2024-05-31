import { useEffect } from "react"
import styles from "./Modal.module.css"
import errorImg from '/error.png'

const Modal = ({active, setActive, children}) => {
    useEffect(() => {
        console.log(active);
    }, [active]);

    
    
    return (
        <div className={active ? `${styles.modal} ${styles.active}` : styles.modal} onClick={(e) => {setActive(false)}}>
            <div className={styles.modalContent} onClick={(e) => {e.stopPropagation()}}>
                {children}
                <img src={errorImg} alt="error" width={800} height={600}/>
            </div>
        </div>
    )
}

export default Modal;