## Panel Manager

`Panel Manager` - система управления игровыми панелями (UI).

## HOW  TO

Вам достаточно создать несколько классов унаследованных от абстрактных панелей, поместить их на префабы и начать пользоваться. Запустите сцену `PanelManager - Examples - Scenes - PanelManagerExample` для демонстрации переключения окон.

Для реализации своего менеджера панелей вам потребуется:

* Создать и унаследовать менеджер от `PanelManagerBase`
* Создать любое количество панелей `ScreenPanelBase` или `OverlayPanelBase` и сделать из них префабы
* Создать файл конфигурации `PanelManagerSettings` и задать в нём ссылки на ваши префабы панелей
* Настроить переходы между панелями

## GET STARTED

1. Импортируйте ассет "PanelManager" в ваш проект. Разместите папку `PanelManager` в любом, удобном для вас месте

2. Создайте C# скрипт и назовите его `PanelManager`

3. Унаследуйте его от `PanelManagerBase`. В данном классе можно переопределить такие методы, как `OpenScreen`, `ClosePanel`, `GetPanel` и другие. Но это совершенно не обязательно. Оставьте его пустым.

   ```c#
   public class PanelManager : PanelManagerBase {}

4. Создайте новую сцену и пустой объект в ней. Назовите этот объект `[PanelManager]` (на самом деле имя не имеет ни какого значения, можете задать ему любое другое). Добавьте на этот объект ранее созданный компонент `PanelManager`.

   Как вы можете заметить, ваш скрипт уже имеет некоторые серилизованные поля.

   * `Settings` - ссылка на `ScriptableObject`, который хранит в себе префабы ваших панелей (вернёмся к нему позже)
   * `Canvas` - это canvas, в который будут помещены все панели.

5. Создание панелей.

   Данный ассет поддерживает 2 типа окон - `Screen` и `Overlay`

   * Screen может быть открыт 1 и только 1 в любой момент времени (открытие следующего экрана закрывает предыдущий)
   * Screen имеет возможность хранить историю и открывать "предыдущий экран"
   * Окна типа Overlay могут быть одновременно открыты без ограничений. Для сортировки порядка отрисовки используется поле `order` (о нём позже).
   * Открытие нового Overlay не закрывает предыдущие

   Создайте новый C# скрипт и унаследуйте его от `ScreenPanelBase` - это ваше первое окно типа `Screen`. Реализуйте базовые методы.

   ```c#
   public class MenuScreen : ScreenPanelBase
   {
       //Вызывается при создании панели для её инициализации
       public override void Initialize() {}
       //Вызывается при каждом открытии панели
       protected override void OnOpened() {}
       //Вызывается при каждом закрытии панели
       protected override void OnClosed() {}
   }
   ```

   Данный клас будет отвечать за отдельное окно. Вы можете разместить необходимые вам серилизованные поля (например, `Button`, `Text` и другие) и реализовать логику поведения окна:

   * Поведение для кликов по UI
   * Отображение нужной информации
   * Логику переходов к другим окнам

   Добавьте ссылку на `Button` и реализуйте метод клика по этой кнопке. Мы вернёмся к нему позже.

   ```c#
   public class MenuScreen : ScreenPanelBase 
   {
       [SerializeField] private Button _button;
   
       public override void Initialize()
       {
           _button.onClick.AddListener(OnClick);
       }
       
       protected override void OnOpened() {}
       protected override void OnClosed() {}
   
       private void OnClick()
       {
           //TODO:
       }
   }
   ```
6. Самого по себе скрипта не достаточно. Для демонтрации работы нам также понадобится префаб данного окна. Создайте пустой объект и назовите его также, как ваш новый скрипт. В нашем случае это `MenuScreen`. Добавьте на него созданный компонент.

   Добавьте кнопку и любые другие дочерние объекты, чтобы вы могли определить этот экран. Задайте ссылку на вашу кнопку в компоненте.
7. Сделайте префаб из вашей панели и удалите его со сцены.
8. Создайте новый C# с именем `PauseOverlay`, как в пункте `8`, только унаследуйте от `OverlayPanelBase`. Это будет другой тип панели, который будет открываться поверх всех остальных окон.

    ```c#
    public class PauseOverlay : OverlayPanelBase // <=====
    {
        public override void Initialize() {}
        protected override void OnOpened() {}
        protected override void OnClosed() {}
    }
    ```
   Добавьте кнопку по аналогии с предыдущим примером. Данная кнопка будет просто закрывать панель, используя базовый метод `Close()`

    ```c#
    public class PauseOverlay : OverlayPanelBase
    {
        [SerializeField] private Button _button;
    
        public override void Initialize()
        {
            _button.onClick.AddListener(OnClick);
        }
    
        protected override void OnOpened() {}
        protected override void OnClosed() {}
    
        private void OnClick()
        {
            Close();
        }
    }
    ```
   Создайте дочерний объект для `Overlays`, добавьте на него созданный компонент, добавьте дочерние элементы (обязательно `Button`) так, как делали в предыдущих пунктах. Для удобства можете добавить `Image` для блока Reycast (ведь `Overlay` будет открываться поверх `Sceen`.

   Обратите внимание, теперь у вашей панели появилось новое серилизованное поле `Order`. Оно отвечает за порядок отрисовки (а именно расположение по иерархии - чем больше, тем ниже) в случае, когда одновременно открыто больше 1 `Overlay`. 
9. Создайте префаб вашего `PauseOverlay` и удалите его со сцены.
10. Чтобы ваши панели появились на сцене, осталось сделать ещё пару шагов. Создайте `ScriptableObject` типа `PanelManagerSettings`. Для этого кликните ПКМ в окне `Project` и выберите `Create -> PanelManager -> Settings`. В выбранном месте будет создан `SO` с именем `PanelManagerSettings`

   Поместите в данный объект ссылки на ваши префабы панелей

   Поместите в объект `[PanelManager]` на сцене ссылку на только что созданный SO

11. Если сейчас запустить сцену, то ничего не произойдет. Сперва необходимо проинициализировать ваш панел менеджер и открыть любую панель. Сделать это можно в любом скрипте, но мы это сделаем в самом `PanelManager`. Добавьте метод `Awake` и вызовите метод `Initialize()`. После инициализации вызовите метод `OpenScreen<MenuScreen>()` - он покажет вам вашу панель.

    ```c#
    public class PanelManager : PanelManagerBase
    {
        private void Awake()
        {
            //Создаст все панели, указанные в PanelManagerSettings
            Initialize();
            
            //Откроет экран
            OpenScreen<MenuScreen>();
        }
    }
    ```

    > Методы `OpenScreen<TY>(bool remember = true) where TY : ScreenPanelBase` и `OpenOverlay<TY>() where TY : OverlayPanelBase` являются обобщеннымы и используются для открытия панелей указанного типа.
    >
    > Если бы мы хотели открыть `PuseOverlay`, то нам потребовалось бы вызвать метод `OpenOverlay<PauseOverlay>()`
    >
    > `OpenScreen` принимает `bool` аргумент, который необходим для открытия предыдущего окна с помощью метода `TryOpenPreviousScreen()` (только если `remember = true`)
    
12. Пришло время запустить сцену. У нас должен быть показан экран `MenuScreen`. Если это так - то вы всё сделали правильно. Если вы нажмёте на кнопку, то ничего не произойдёт - ведь мы не настроили логику работы.
13. Зайдите в класс `name` и реализуйте метод `OnClick`, отвечающий за поведение кнопки. Мы хотим открыть оверлей паузы с помощью метода `OpenOverlay<PauseOverlay>()`.

    ```c#
    public class MenuScreen : ScreenPanelBase
    {
        [SerializeField] private Button _button;
    
        public override void Initialize()
        {
            _button.onClick.AddListener(OnClick);
        }
        
        protected override void OnOpened() {}
        protected override void OnClosed() {}
    
        private void OnClick()
        {
            _panelManager.OpenOverlay<PauseOverlay>();
        }
    }
    ```

    > Для переключения панелей нам необходим `PanelManager`. Каждая панель имеет `protected PanelManager _panelManager`  поле, позволяющее обратиться к её менеджеру панелей.

14. Теперь при запуске сцены и клику по кнопке откроется наш оверлей! А при нажатии кнопки `Continue` на оверлее он просто закроется!

## PanelAnimationBase

Помимо обычный панелей есть панели с анимациями открытия и закрытия.

`PanelAnimationBase` - наследуется от `PanelBase`, содержит все его методы, дополнен своей реализацией и содержит базовые поля:
* `AnimatedObject` - Transform бъекта к которому будет применятся анимация
* `OpenPanelAnimationParameters` - анимация открытия окна
* `ClosePanelAnimationParameters` - анимация закрытия окна

Анимации открытия и закрытия окна являются классом `PanelAnimationParameters`, который управляет и создает анимации по ключу, содержит следующие поля:
* `AnimationKey` - ключ по которому будет создан класс аниматор окна.
* `Animation Curve` - график по которому будет происходить изменение окна, где начальная точка это параметры окна в начале анимации, а конечная точка это
параметры окна в конце анимации. При параметре None, анимация у такого окна будет игнорироваться.

Чтобы воспользоваться панелью с анимацией создайте свой класс и отнаследуйте его от `PanelAnimationBase`, для корректной работы вызовите в методе `OnInitialize` базовую реализацию этого метода,
для инициализации анимаций. В остальном все взаимодействие происходит так же как и с наследниками `PanelBase`.
    
```c#
    public class AnimatedPanel : PanelAnimationBase
    {     
        public override PanelType PanelType         => PanelType.Screen;
        public override bool      RememberInHistory => true;

        protected override void OnInitialize()
        {
            base.OnInitialize();       
        }

        protected override void OnOpened() {}

        protected override void OnClosed() {}
    }   
```
## Created new PanelNativeAnimation

Для создания новой анимации окна, создайте класс и отнаследуйте его от `PanelNativeAnimationBase`, это базовый класс анимации окна, он содержит в себе следующие методы:

```c#
   
        public event Action AnimationEnded;
        protected Tween _animationTween;
        
        //Метод запуска анимации принимает Transform объекта к которому будет применятся анимаця, и график изменения анимации со временем
        public virtual void StartAnimation(Transform animationTransform, AnimationCurve curve) {}
        
        //Метод остановки анимации
        public virtual void StopAnimation()
        
        //Метод установки параметров до применения анимации
        public virtual void SetDefault() 
        
        //Очистка памяти от анимации
        protected void KillAnimation()
        
```
Для своей реализации переорпеделите нужные вам методы.

После написания своей анимации необходимо ее проинициализировать в коде, откройте файл `PanelAnimationKey.cs`, впишите туда название вашей анимации, после этого откройте файл
`PanelAnimationParameters` найдите там метод `CreatePanelAnimation` и в switch внутри этого метода добавьте case  с вашей анимацией, затем откройте Editor и настройте анимацию
у вашего окна.

## FEATURES AND API

Если вы хотите, чтобы перед открытием какого либо окна в него были переданы какие-либо данные, то вам необходимо реализовать метод `AcceptArg<T>(T arg)` интерфейса  `IAcceptPanelArg<T>`.

> Это относится к любому типу панелей: как `MainMenuScreenDemo : ScreenPanelBase, IAcceptPanelArg<string>`  так и `MainMenuOverlayDemo : OverlayPanelBase, IAcceptPanelArg<string>`

```c#
public class MainMenuScreenDemo : ScreenPanelBase, IAcceptPanelArg<string> {
    public override void Initialize() {}
    protected override void OnOpened() {}
    protected override void OnClosed() {}
    public void AcceptArg(string arg) {}
}
```

В таком случае, вы сможете передать параметры и открыть панель с помощью следующих методов (зависит от типа панели):

* `IPanelManager.OpenScreen<TY, TZ>(TZ arg, bool remember = true) where TY : ScreenPanelBase, IAcceptPanelArg<TZ>`
* `IPanelManager.OpenOverlay<TY, TZ>(TZ arg) where TY : OverlayPanelBase, IAcceptPanelArg<TZ>`

Например: `PanelManager.OpenScreen<MainMenuScreenDemo, string>("my string")`

Ниже представлено описание интерфейса `IPanelManager`

```c#
public interface IPanelManager {
	//Открывает Overlay с типом TY
    void OpenOverlay<TY>() where TY : OverlayPanelBase;
    
    //Открывает оверлей с типом TY и передает аргумент TZ
    void OpenOverlay<TY, TZ>(TZ arg) where TY : OverlayPanelBase, IAcceptPanelArg<TZ>;

	//Открывает Screen с типом TY. remember позволяет поместить экран в историю для возможности вызова `TryOpenPreviousScreen`
    void OpenScreen<TY>(bool remember = true) where TY : ScreenPanelBase;
    
    //Открывает Screen с типом TY и передает аргумент TZ
    void OpenScreen<TY, TZ>(TZ arg, bool remember = true) where TY : ScreenPanelBase, IAcceptPanelArg<TZ>;

	//Закрывает панель с типом TY
    void ClosePanel<TY>() where TY : PanelBase;
    
    //Закрывает текущий Screen
    void CloseCurrentScreen();
    
    //Закрывает все Overlay
    void CloseAllOverlays();

    //Открывает предыдущий Screen. Возвращает false, если нет сохраненных для открытия экранов
    bool TryOpenPreviousScreen();
    
    //Очищает историю открытий Screen
    void ClearHistory();
}
```

> В наших демонстрационных примерах вы можете найти примеры использования данных функций



