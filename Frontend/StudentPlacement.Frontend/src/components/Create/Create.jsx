
import { useState } from "react";
import styles from "./Create.module.css"
import Account from "../Account/Account";
import Structure from "../StuructureUniversity/Strucrute";

const Create = ({SetTab}) => {
    const [tab, setTab] = useState("Account");


    return (
        <main className={styles.main}>
            <nav className={styles.navigate}>
                <ul>
                    <li className={tab == "Account" ? styles.navigateBtnChecked : styles.navigateBtn} onClick={() => {setTab("Account")}}>Пользователи</li>
                    <li className={tab == "Structure" ? styles.navigateBtnChecked : styles.navigateBtn} onClick={() => {setTab("Structure")}}>Структура</li>
                </ul>
            </nav>
            <section className={styles.view}>
                {tab == "Account" && (<Account SetTab={SetTab}/>)}
                {tab == "Structure" && (<Structure/>)}
            </section>
        </main>
    )
};

export default Create;