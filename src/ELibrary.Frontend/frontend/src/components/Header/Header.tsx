import {useEffect, useState} from 'react';
import  styles  from './header.module.css';
import * as IconsPath from '../../icons/icons'
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../hook/useAuth';
import { client } from '../../client/client';

export function Header() {
    const [userIcon, setUserIcon] = useState<{id: number, img: string}>({
        id: 0,
        img: ''
    })
    const [isShowMenu, setShow] = useState<boolean>(false)
    const authStore = useAuth()
    const navigate = useNavigate()
    const [isSearch, setSearch] = useState<boolean>(false)
    const [searchValue, setSearchValue] = useState<string>('')
        
     const logout =  async () => {
        console.log(userIcon)
        await client.get('auth/logout/' + authStore.user.Id)
        .then(response => {
            console.log(response.data)
            authStore.removeUser()
            setShow(false)
            navigate('/')
        })
        .catch(error => {
            console.log(error)
        })
    }
    
    useEffect(() => {
        if (authStore.isAuth) {
            // console.log(toJS(authStore.user))
            // console.log('user/get-image/' + authStore.user.id)
            const getImg = async (id: number) => {
                // console.log('user/get-image/' + id)
                await client.get('user/get-image/' + id)
                . then(response => {
                    // console.log(response.data)
                    // console.log(userIcon)
                    setUserIcon({id: id, img: response.data})
                })
                .catch(error => {
                    console.log('', error)
                })
            }
            // console.log(authStore.user.Id)
            getImg(authStore.user.Id)
        }
        else {
            setUserIcon({id: -1, img: ''})
        }
    }, [authStore.isAuth, authStore.user.Id])
    const handleChange = (e) => {
        e.preventDefault();
        setSearchValue(e.target.value)
    }

    const onEnterPress =  (e) => {
        if(e.key == 'Enter') {
            if (searchValue.length < 4) {
                return
            }
            navigate('/search/?name=' + searchValue)
            setSearch(false)
        }
    }
    //const [isModalOpen, setIsModalOpen] = useState(false);
    // const [isAuth, setAuth] = useState(props.isAuth)
    // const id = localStorage.getItem('id');
    //const [open, setOpen] = useState(false);
    //const [seen, setSeen] = useState(false)
    //const openModal = () => setIsModalOpen(true);
    //const closeModal = () => setIsModalOpen(false)
    return (
       <div className={styles.header}>
            <div className={styles.header__menu}>
                <Link to='/'><button className={styles.menu_button}>Главная</button></Link>
                {/* <div> */}
                <Link to='/genre/all'><button className={styles.menu_button}>Жанры</button></Link>
                {/* {open ? <DropdownList toggle={toogle} /> : null} */}
                {/* </div> */}
                <Link to='/user/authors'><button className={styles.menu_button}>Авторы</button></Link> 
                <button className={styles.menu_button} onClick={() => {navigate('book/')}}>Все книги</button>         
            </div>
            <div  className={styles.header__menu_right}>
                {isSearch ? 
                    <div className={styles.search_block}>
                        <input value={searchValue} onKeyDown={onEnterPress} onChange={handleChange} minLength={4} className={styles.search_input} />
                        <img className={styles.search_close_icon} src={IconsPath.crossIcon} onClick={() => setSearch(false)} />
                    </div>
                :
                    <img className={styles.search_icon} src={IconsPath.searchIcon} onClick={() => setSearch(true)}/>}
                {/* {isAuth ? <Link to ='/lks'><button className={styles.menu_button}>Личный кабинет</button></Link> :  */}
                {userIcon.id == -1 ? 
                <button onClick={() => {navigate('/auth', {replace: true})}} className={styles.menu_button}>Войти</button>
                 : <div onClick={() => {setShow(!isShowMenu)}}>
                    <img className={styles.lks_icon} src={userIcon.img}/></div>}
                {isShowMenu ?
                <div className={styles.dropdown_menu}>
                    <ul>
                        <li onClick={() => {navigate('/lks', {replace:true})}}><span>Профиль</span></li>
                        <li onClick={() => {navigate('/user/favorite')}}><span>Закладки</span></li>
                        <li onClick={logout}><span>Выйти</span></li>
                    </ul>
                </div> : 
                null}
                {/* <Link to='/auth'><button className={styles.menu_button}>Войти</button></Link> */}
                {/* } */}
                {/* <button onClick={togglePop}>Login</button> */}
                {/* {seen ? <AuthPopup toggle={togglePop} /> : null} */}
                {/* <AuthPopup toggle={togglePop}/> */}
            </div>
       </div>
      );
}

