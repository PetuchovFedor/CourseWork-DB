import styles from './Main.module.css'

export function Main() {

    return (
        <div className={styles.main_block}>
            <h2 className={styles.title}>Добро пожаловать в электронную библиотеку!</h2>
            <p className={styles.paragraph}> Откройте для себя уникальную возможность скачивать книги на свое устройство, где бы вы ни находились.</p>
            <p className={styles.paragraph}>Присоединяйтесь к нашему сообществу чтения, зарегистрируйтесь и делитесь своими собственными литературными произведениями.</p>
            <p className={styles.paragraph}>У нас вы найдете богатый выбор книг по различным тематикам, а также сможете предложить свои собственные творческие работы для чтения и обсуждения.</p>
            <p className={styles.paragraph}> Присоединяйтесь к нам и окунитесь в мир книг прямо сейчас!</p>


        </div>
    )
}