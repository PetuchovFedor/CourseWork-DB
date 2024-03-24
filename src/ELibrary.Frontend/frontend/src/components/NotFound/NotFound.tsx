import { useNavigate } from 'react-router-dom'
import styles from './NotFound.module.css'

export function NotFound() {
    const navigate = useNavigate()
    return(
        <div className={styles.not_found_block}>
            <h2 className={styles.title}>404. Страница не найдена</h2>
            <p className={styles.label}>Страница устарела, была удалена или не существовала вовсе.</p>
            <button className={styles.button} onClick={() => navigate('/')}>Вернуться на главную страницу</button>
        </div>
    )
}