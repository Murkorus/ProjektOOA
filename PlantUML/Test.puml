@startuml
class Program
Program : - {static} void Main(string[] args)
Program : - float frameTime
Program : - void NextFrame()
Program : - void Click()
Program -- Beanstalk
Program -- GraphicsManager
Program -- UpgradeManager

class Beanstalk
Beanstalk : + {static} float height
Beanstalk : + {static} float beans
Beanstalk : + {static} float beansPerSecond
Beanstalk : + {static} float heightPerSecond
Beanstalk : + {static} float heightPerClick
Beanstalk : + {static} Dictionary[string, int] acquiredUpgrades

class GraphicsManager
GraphicsManager : + {static} void PrintBeanstalk()
GraphicsManager : + {static} int selectedUIElement
GraphicsManager : + {static} void PrintUIElement(int id)
GraphicsManager -- Beanstalk : prints data

class UpgradeManager
UpgradeManager : + {static} void BuyUpgrade(int index)
UpgradeManager : + {static} Upgrade[]
UpgradeManager -- Beanstalk
UpgradeManager o-- Upgrade : has 0.* >

class Upgrade
Upgrade : + string name
Upgrade : + float cost
Upgrade : + int maxCount
Upgrade : - float beansPerSecond
Upgrade : - float heightPerSecond
Upgrade : - float heightPerClick
Upgrade : + Upgrade()
@enduml