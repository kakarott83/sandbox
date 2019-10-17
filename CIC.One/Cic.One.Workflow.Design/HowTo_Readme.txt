Icons:
Toolbox: Damit das Icon in der Toolbox angepasst wird, ist die Activity um
[System.Drawing.ToolboxBitmap(typeof(IconResourceAnchor), "QuestionMark.png")]
zu erweitern. IconResourceAnchor muss eine Klasse aus dem Ordner sein, in dem das Bild liegt.
Sollte es eine Erweiterung über IActivityTemplateFactory geben, so ist das Attribut stattdessen dort abzulegen!

Activity:
Im ActivityDesigner(.xaml.cs) wird nach InitComponent
this.Icon = ResourceLoading.loadIcon("QuestionMark.png"); aufgerufen, welches das Bild als Brush erzeugt. Das Laden erfolgt wie bei der Toolbox über eine Hilfklasse.


Default-Value vorbelegen im Designer:
Eine Klasse von gleichem Namen wie die Activity, extends IActivityTemplateFactory
public Activity Create(DependencyObject target)
{
    return new Cic.One.Workflow.Activities.AddMessage
    {
        wfcontext = new InArgument<WorkflowContext>(new VisualBasicValue<WorkflowContext>("input"))

    };
}



Neue Activity:
* Cic.One.Workflow.Activities - Activity kopieren, umbenennen
* in/out Parameter festlegen, ggf. [RequiredArgument]
* Execute implementieren
* Icon in Icons anlegen
* In Cic.One.Workflow.Design AddMessageDesign.cs kopieren und in <ActivityName>Design.cs umbenennen.
* Iconname darin anpassen
* Ein .xaml kopieren und umbenennen und den Designer bauen. Vorwiegend über XAML, nicht gut graphisch möglich.
* In ActivityLibraryMetadata.cs den Designer registrieren:
	builder.AddCustomAttributes(typeof(Cic.One.Workflow.Activities.<ActivityName>), new DesignerAttribute(typeof(<ActivityName>Designer)));