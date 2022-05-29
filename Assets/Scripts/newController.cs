using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newController : MonoBehaviour
{
     //переменная для установки макс. скорости персонажа
    public float maxSpeed = 10f;
    public float jumpForce = 300f;
    //переменная для определения направления персонажа вправо/влево
    private bool isFacingRight = true;
    //ссылка на компонент анимаций
    private Animator anim;
	//находится ли персонаж на земле или в прыжке?
	private bool isGrounded = false;
	private bool isAttacking = false;
	
	//ссылка на компонент Transform объекта
	//для определения соприкосновения с землей
	public Transform groundCheck;
	//радиус определения соприкосновения с землей
	private float groundRadius = 0.2f;
	//ссылка на слой, представляющий землю
	public LayerMask whatIsGround;
	private Rigidbody2D _rigidbody2D;
	[SerializeField] private GameObject attackHitBox;
	private float _takeDamageCd = 2f;
	private float _currentTakeDamageCd;

	public int health = 100;

    /// <summary>
    /// Начальная инициализация
    /// </summary>
	private void Start()
    {
        anim = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        attackHitBox.SetActive(false);
    }
	
    /// <summary>
    /// Выполняем действия в методе FixedUpdate, т. к. в компоненте Animator персонажа
    /// выставлено значение Animate Physics = true и анимация синхронизируется с расчетами физики
    /// </summary>
	private void FixedUpdate()
    {
		//определяем, на земле ли персонаж
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround); 
		//устанавливаем соответствующую переменную в аниматоре
		anim.SetBool ("isJumping", !isGrounded);
		//устанавливаем в аниматоре значение скорости взлета/падения
		anim.SetFloat ("ySpeed", _rigidbody2D.velocity.y);
        //если персонаж в прыжке - выход из метода, чтобы не выполнялись действия, связанные с бегом
        // if (!isGrounded)
        //     return;
        //используем Input.GetAxis для оси Х. метод возвращает значение оси в пределах от -1 до 1.
        //при стандартных настройках проекта 
        //-1 возвращается при нажатии на клавиатуре стрелки влево (или клавиши А),
        //1 возвращается при нажатии на клавиатуре стрелки вправо (или клавиши D)
        float move = Input.GetAxis("Horizontal");

        //в компоненте анимаций изменяем значение параметра Speed на значение оси Х.
        //приэтом нам нужен модуль значения
        anim.SetFloat("Speed", Mathf.Abs(move));

        //обращаемся к компоненту персонажа RigidBody2D. задаем ему скорость по оси Х, 
        //равную значению оси Х умноженное на значение макс. скорости
        _rigidbody2D.velocity = new Vector2(move * maxSpeed, _rigidbody2D.velocity.y);

        //если нажали клавишу для перемещения вправо, а персонаж направлен влево
        if(move > 0 && !isFacingRight)
            //отражаем персонажа вправо
            Flip();
        //обратная ситуация. отражаем персонажа влево
        else if (move < 0 && isFacingRight)
            Flip();
    }

	private void Update()
	{
		//если персонаж на земле и нажат пробел...
		if (isGrounded && Input.GetKeyDown(KeyCode.W)) 
		{
			//устанавливаем в аниматоре переменную в false
			
			if (isAttacking == false)
				anim.SetBool("isJumping", true);
			//прикладываем силу вверх, чтобы персонаж подпрыгнул
            _rigidbody2D.AddForce(new Vector2(0, jumpForce));				
		}

		if (Input.GetKeyDown(KeyCode.K) && !isAttacking && isGrounded)
		{
			isAttacking = true;
			int choose = UnityEngine.Random.Range(1, 4);
			// if (!isGrounded)
			// {
			// 	anim.Play("attack_fly" + choose);
			// 	Debug.Log("fikus");
			// }
			// else
			anim.Play("attack" + choose);
			StartCoroutine(DoAttack());
		}

		_currentTakeDamageCd += Time.deltaTime;
	}

    /// <summary>
    /// Метод для смены направления движения персонажа и его зеркального отражения
    /// </summary>
    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
        var transform1 = transform;
        Vector3 theScale = transform1.localScale;
        //зеркально отражаем персонажа по оси Х
        theScale.x *= -1;
        //задаем новый размер персонажа, равный старому, но зеркально отраженный
        transform1.localScale = theScale;
    }

    IEnumerator DoAttack()
    {
	    attackHitBox.SetActive(true);
	    yield return new WaitForSeconds(0.5f);
	    attackHitBox.SetActive(false);

	    isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
	    if (col.gameObject.layer == 8)
	    {
		    if (health > 0 && _currentTakeDamageCd >= _takeDamageCd)
				health -= 20;
		    else
		    {
			    health = 0;
		    }
	    }
    }
}
