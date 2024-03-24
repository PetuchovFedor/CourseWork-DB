import { useNavigate } from 'react-router-dom'
import styles from './ImageBlock.module.css'
import { RedirectEnum } from './RedirectEnum'

interface IImageBlockProps {
    id: number,
    name: string,
    img: string
    width: number,
    height: number
    redirect: RedirectEnum
}

export function ImageBlock(props: IImageBlockProps) {
    const navigate = useNavigate()
    const blockStyle = {
        width: props.width,
        height: props.height,        
    }

    const imageStyle = {
        width: props.width - 50,
        height: props.height - 50,    
    }
    const navigateHandle = () => {
        switch (props.redirect)
        {
            case RedirectEnum.user:
            {
                navigate('/user/' + props.id)
                break
            }               
            case RedirectEnum.book:
            {
                navigate('/book/' + props.id)
                break
            }
        }
    }
    // const imageStyle = {
    //     backgroundImage: 'url(' + props.img + ')',
    // }
    return(
        <div className={styles.block}style = {blockStyle} onClick={navigateHandle}>
            <img style={imageStyle} src={props.img} />
            {/* <div style = {imageStyle} className={styles.image}></div> */}
            <span>{props.name}</span>
        </div>
    )
}