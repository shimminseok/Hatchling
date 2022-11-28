using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] string _hitObjTag;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_hitObjTag))
        {
            switch (_hitObjTag)
            {
                //플레이어를 때렷을때
                case "Player":
                    CharacterCtrl charctrl = GameManager._instance.character;
                    MonsterCtrl mon = gameObject.GetComponentInParent<MonsterCtrl>();
                    mon._hitboxTf.gameObject.GetComponent<BoxCollider>().enabled = false;

                    charctrl.HittingME(mon.finalDam);
                    if (charctrl._target == null)
                    {
                        charctrl._target = mon.gameObject;
                    }

                    break;
                //몬스터를 떄렷을때
                case "Enemy":
                    CharacterCtrl owner = gameObject.GetComponentInParent<CharacterCtrl>();
                    owner._hitboxTf.gameObject.GetComponent<BoxCollider>().enabled = false;
                    if (other.gameObject == owner._target.gameObject)
                    {
                        if (owner._target.gameObject == null)
                        {
                            return;
                        }
                        MonsterCtrl monctrl = owner._target.gameObject.GetComponent<MonsterCtrl>();
                        monctrl._target = owner.gameObject;
                        monctrl.HittingME(owner.finalDam);

                    }
                    else
                    {
                        return;
                    }
                    break;
            }
        }
    }
}
