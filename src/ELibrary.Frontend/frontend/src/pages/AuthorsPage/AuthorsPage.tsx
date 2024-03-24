import { Link, useParams } from "react-router-dom";
import axios from "axios";
import { useEffect, useState } from "react";
import { backendUrl } from "../../BackendUrl";
import { ImageBlock } from "../../components/ImageBlock/ImageBlock";
import { IconDto } from "../../dto/IconDto";
import { client } from "../../client/client";
import InfiniteScroll from "react-infinite-scroll-component";
import { RedirectEnum } from "../../components/ImageBlock/RedirectEnum";

export function AuthorsPage() {

    const [countUsers, setCountUsers] = useState<number>(0)
    const [hasMore, setHasMore] = useState<boolean>(true)
    const [scipped, setScipped] = useState<number>(0)
    const [users, setUsers] = useState<IconDto[]>([])
    
    const getAuthors = async (skip: number) => {
        await client.get('user/get-authors/?scipped=' + skip)
        .then((response) => {
            if (response.data.users.length == 0) {
                setHasMore(false)
            }
            setUsers(users.concat(response.data.users))
            setCountUsers(response.totalNumber)
            console.log(data)
        })
        .catch(error => {
          console.log(error.response.data)
        })
    }

    useEffect(() => {
        const getData = async () => {             
            await client.get('user/get-authors/?scipped=' + scipped)
            .then((response) => {
                setUsers(response.data.users)
                if (response.data.users.length == 0) {
                    setHasMore(false)
                }
                setCountUsers(response.totalNumber)
                console.log(data)
            })
            .catch(error => {
            console.log(error.response.data)
            })
        }
        getData()
    }, [])

    const loadMore = async () => {   
        const tempScip =  scipped + 15  
        setScipped(tempScip)
        if (tempScip > countUsers) {       
             setHasMore(false)
             return
        }
        await getAuthors(tempScip)
     }

    const style = {
        display: 'flex',
        flexDirection: 'row',
        flexWrap: 'wrap',
        overflow: 'visible',
        background: 'white',
        padding: '10px',
        width: '80%',
        marginLeft: 'auto',
        marginRight: 'auto',
        marginTop: '30px'
    }
    return (
        <> {users.length == 0 ? null :
            // <div className={styles.books_section}>
            <InfiniteScroll
            // className={styles.books_section}
            dataLength={users.length}
            next={loadMore}
            hasMore={hasMore}
            loader={null}
            style={style}
            >
            
            {users.map(user => {
                return <ImageBlock id={user.id} name={user.name} img={user.img} 
                width={200} height={200} redirect={RedirectEnum.user} />
            })}
            </InfiniteScroll>
        // </div>
    }
        </>        
    )
}