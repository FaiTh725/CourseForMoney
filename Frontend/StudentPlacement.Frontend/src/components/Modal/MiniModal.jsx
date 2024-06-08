import { act, useEffect, useState } from "react"
import styles from "./MiniModal.module.css"


const MiniModal = ({ active, setActive, children }) => {

    useEffect(() => {
        console.log(active);
        if (active == true) {
            const timer = setTimeout(() => {
                setActive(false);
            }, 3000);
    
            return () => clearTimeout(timer);
        }
    }, [active]);


    return (
        <div className={active ? `${styles.modal} ${styles.active}` : styles.modal}>
            <div className={styles.modalContent} onClick={(e) => { setActive(false); console.log("click") }}>
                {children}
            </div>
        </div>
    )
}

export default MiniModal;