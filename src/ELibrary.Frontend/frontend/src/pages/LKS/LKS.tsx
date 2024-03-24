import { useState, useRef, useLayoutEffect, useEffect } from 'react';
import styles from "./LKS.module.css"
import { editIcon } from '../../icons/icons';
import { Link, useNavigate, useSearchParams } from 'react-router-dom';
import { LksDto } from '../../dto/User/LksDto';
import { useAuth } from '../../hook/useAuth';
import { client } from '../../client/client';
import { UpdateAboutDto, UpdateLks, UpdatePassword, UpdatePhoto } from '../../dto/User/UpdateUser';
import { error } from 'console';
// import { AuthorListBook } from '../../components/ListBook/ListBooks';
import { FilterAndSortingPanel } from '../../components/FilterAndSortingPanel/FilterAndSortingPanel';
import { SortState } from '../../components/FilterAndSortingPanel/SortState';
import { ListBooks } from '../../components/ListBooks/ListBooks';
import { SearchParams, parseUrl } from '../../hook/parseUrl';

interface Passwords {
    old: string,
    new: string
}

interface NameAndEmail {
    name: string,
    mail: string
}
// const getEditableTextArea = (textAreas: NodeListOf<HTMLTextAreaElement>) => {
//     for (const item in textAreas) {
//         if (!textAreas[item].readOnly) {
//             return textAreas[item]
//         }
//     }
// }
export function LKS() {
    // const aboutRef = useRef(null)
    const [nameAndEmail, setNameAndEmail] = useState<NameAndEmail>({
        name: '',
        mail: ''
    })
    const [seenAbout, setSeenAbout] = useState<boolean>(true)
    const [about, setAbout] = useState<string>('')
    const [seenMybooks, setSeenMybooks] = useState<boolean>(false)
    const [seenSetting, setSeenSetting] = useState<boolean>(false)
    const authStore = useAuth()
    const [error, setError] = useState()
    const navigate = useNavigate()
    const [data, setData] = useState<LksDto>({} as LksDto)
    const [searchParams, setSearchParams] = useSearchParams()
    const [params, setParam] = useState<SearchParams>({
        sortState: SortState.Error,
        genresIds: []
    })
    const [passwords, setPasswords] = useState<Passwords>({
        old: '',
        new: ''
    })
    // parseUrl()
    const imgInputRef = useRef<HTMLInputElement>(null)
    // const textArea = document.querySelector('textarea')
   
    // const textRowCount = textArea ? textArea.value.split("\n").length : 0
    // const rows = textRowCount + 1

    const [seenAboutText, setSeenAboutText] = useState<boolean>(true)

    useEffect(() => {
        const  getData = async (id: number) => {
            await client.get<LksDto>('user/lks/' + id)
            .then(response => {
                setData(response.data)
                setAbout(data.about === null ? '' : data.about)
                setNameAndEmail({name: data.name, mail: data.email})
                //searchParams.getAll()
                const sortState = searchParams.get('sort-state')
                const genres = searchParams.getAll('genres-ids')
                const params = parseUrl(sortState, genres)
                console.log('params', params)
                setParam(params)
                params.sortState !== SortState.Error || params.genresIds.length != 0 ? clickOnMyBooks() : clickOnProfile()
                console.log(response.data)
            })
            .catch(error => {
                console.log(error.response.data)
            })
        }
        if (!authStore.isAuth) {
            navigate('/auth')
        }
        getData(authStore.user.Id)
    }, [authStore.isAuth])
    const styleIcon = {
        // backgroundImage: 'url(' + data. + ')',
    };

    const openSelectImageModal = () => {
        if (imgInputRef.current) {
            imgInputRef.current.click()
        }
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const{name , value} = e.target;
        setPasswords({...passwords, [name]: value});
    }

    const change = (e: React.ChangeEvent<HTMLInputElement>) => {
        const{name , value} = e.target;
        setNameAndEmail({...nameAndEmail, [name]: value});
    }

    const selectedButton = {
        borderBottom: '3px solid blue',
    }

    const unSelectedButton = {
        borderBottom: 'none',       
    }

    const clickOnProfile = () => {
        setSeenAbout(true)        
        setSeenMybooks(false)
        setSeenSetting(false)
    }
    
    const clickOnMyBooks = () => {
        setSeenAbout(false)        
        setSeenMybooks(true)
        setSeenSetting(false)
    }

    const clickOnSettings = () => {
        setSeenAbout(false)        
        setSeenMybooks(false)
        setSeenSetting(true)
    }

    const updateAbout = async (e) => {
        e.preventDefault()        
        // setAboutValue(aboutRef.current.value)
        // setSeenAboutText(true)
        // data.about = aboutRef.current.value
        const dto: UpdateAboutDto = {id: data.id, about: about}
        await client.put('user/update-about', dto)
        .then(response => {
            setData({...data, about: about})
            console.log(response.data)
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }

    const updatePhoto = async () => {
        if (imgInputRef.current && imgInputRef.current.files) {
            const img = imgInputRef.current.files[0]
            const dto: UpdatePhoto = {id: data.id, file: img}
            await client.put('user/update-image', dto, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
            .then(response => {
                console.log(response.data)
                window.location.reload()
            })
            .catch(error => {
                console.log(error.response.data)
            })
        }
    }
    const deleteUser = async () => {
        await client.delete('user/delete/' + data.id)
        .then(response => {
            authStore.removeUser()
            navigate('/')
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }
    const updatePassword = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        const dto: UpdatePassword ={ id: data.id, oldPassword: passwords.old,
            newPassword: passwords.new}
        await client.put('user/update-password', dto)
        .then(response => {
            console.log(response.data)
            setPasswords({old: '', new: ''})
        })
        .catch(error => {
            console.log(error.response.data)
            setError(error.response.data)
        })
    }

    const updateLks = async (e) => {
        e.preventDefault()
        const dto: UpdateLks = { 
            id: data.id,
            name: nameAndEmail.name,
            email: nameAndEmail.mail
        }
        await client.put('user/edit-lks', dto)
        .then(response => {
            setData({...data, name: nameAndEmail.name, email: nameAndEmail.mail})
            console.log(response.data)
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }

    return (
        <>
            <div className={styles.profile}>          
                <div className={styles.profile_header}>
                    <div onClick={openSelectImageModal}>
                        <img src={data.photoPath} className={styles.profile_header_icon}/>                    
                    </div>
                    <div className={styles.profile_header_content}>
                        <h2>{data?.name}</h2>
                    <button className={styles.profile_settings} onClick={clickOnSettings}>Настройки</button>
                    </div>
                </div>
                <div className={styles.profile_content}>
                    <div className={styles.content_menu}>
                        <button style={seenAbout ? selectedButton : unSelectedButton} className={styles.content_menu_button}
                        onClick = {clickOnProfile}>                            
                            Профиль
                        </button>
                        <button style={seenMybooks ? selectedButton : unSelectedButton} className={styles.content_menu_button}
                            onClick = {clickOnMyBooks}
                            >
                            Мои книги
                        </button>
                    </div>
                    <div className={styles.content_section}>
                        {seenAbout ? 
                        <div className={styles.section_about}>
                            <div className={styles.info}>
                                <p>Почта: {data.email}</p>
                                <p>Дата регистрации: {data.dateRegistration === undefined ? null : 
                                data.dateRegistration.substring(0, 10)}</p>
                            </div>
                            <form className={styles.about_form}>
                                <label className={styles.label_about}>О себе:</label>
                                {seenAboutText ?
                                <div className={styles.about_no_edit}>
                                    <img className={styles.edit_icon} src = {editIcon} 
                                    onClick={() => setSeenAboutText(false)} />
                                    <textarea className={styles.textarea_about}
                                    readOnly maxLength={500} defaultValue={data.about === null ? '' : data.about}
                                    ></textarea>                                   
                                </div> :
                                <div className={styles.about_edit}>
                                    <textarea className={styles.textarea_about}
                                    // style={{ minHeight: "6vh", height: "unset" }}
                                    value={about} maxLength={500} 
                                    onChange={(event: React.ChangeEvent<HTMLTextAreaElement>) => {
                                        setAbout(event.target.value)}}
                                   ></textarea>                                
                                    <div>
                                        <button className={styles.exit_button} onClick={() => setSeenAboutText(true)}
                                        >Отмена</button>
                                        <button className={styles.save_button} onClick={updateAbout}>Сохранить</button>
                                    </div>
                                </div>}
                                                                   
                            </form>
                            <button className={styles.add_book_button} onClick={() => {navigate('/add_book')}}>Добавить книгу</button>                          
                        </div> : null}                        
                        {seenMybooks ? 
                        <>
                        <FilterAndSortingPanel sortState={params.sortState == SortState.Error ? SortState.None 
                        : params.sortState} genres={
                            params.genresIds
                        } />
                        <ListBooks genresIds={params.genresIds} sortState={params.sortState == SortState.Error ?
                         SortState.None 
                        : params.sortState} authorId={data.id} readerId={-1}/>
                        </>
                         : null}
                        {/* <AuthorListBook userBooks={data.userBooks} /> : null} */}
                        {seenSetting ? 
                        <div className={styles.section_setting}>
                            <form onSubmit={updatePassword} className={styles.settings_form}>
                                <label className={styles.settings_label}>Сменить пароль</label>
                                <input name='old' type='password' className={styles.settings_input} value={passwords?.old} 
                                onChange={handleChange} placeholder='Старый пароль'/>
                                <span className={styles.error}>{error}</span>
                                <input name='new' type='password' className={styles.settings_input}  value={passwords?.new}
                                onChange={handleChange} placeholder='Новый пароль' />
                                <button className={styles.settings_button} type='submit'>Применить</button>
                            </form>
                            <div className={styles.setting__change_name_email}>
                                <label className={styles.settings_label}>Сменить имя и почту</label>
                                <input name='mail' type='email' className={styles.settings_input} value={nameAndEmail.mail} 
                                onChange={change} placeholder='email' required pattern='/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i'/>
                                <input name='name' type='text' className={styles.settings_input} value={nameAndEmail.name} 
                                onChange={change} placeholder='name' required/>
                                <button onClick={updateLks} className={styles.settings_button}>Применить</button>
                            </div>
                            <button className={styles.button_delete} 
                            onClick={deleteUser}>Уничтожить аккаунт</button>
                        </div> : null}
                        <input ref={imgInputRef} type="file" accept=".png, .jpg,.jpeg" 
                        style={{'display': 'none'}} onChange = {updatePhoto}/>
                    </div>
                </div>
            </div>  
        </>
    )
}