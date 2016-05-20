using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class WebformEmail: MonoBehaviour
{

    public void Run()
    {
        StartCoroutine(upload());
    }

    IEnumerator upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("apollo13-contact-name", "testName");
        form.AddField("apollo13-contact-email", "test@email.com");
        form.AddField("apollo13-contact-subject", "test subject");
        form.AddField("apollo13-contact-content", "test content");
        form.AddField("apollo13-contact-form", "send");
        WWW w = new WWW(@"http://www.vz777.com/contact-2/", form);

        yield return w;

        if (w.error == null)
        {
            print("no error");
            print(w.text);
        }
        else
            print(w.error);
    }

    public bool IsValidEmailAddress(string s)
    {
        var regex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");

        return regex.IsMatch(s);
    }
}